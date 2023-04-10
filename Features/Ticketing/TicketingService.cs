using LinqKit;
using ERP.Shared.Commons;
using ERP.Shared.Ticketing;
using ERP.Ticketing.HttpApi.Data;
using Microsoft.EntityFrameworkCore;
using ERP.Ticketing.HttpApi.Commons.Exceptions;
using ERP.Ticketing.HttpApi.Commons.Extensions;
using ERP.Ticketing.HttpApi.Features.Ticketing.Mappers;
using Mapster;

namespace ERP.Ticketing.HttpApi.Features.Ticketing;

public class TicketingService
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly TicketingCommentService _commentService;

    public TicketingService(AppDbContext dbContext, IHttpContextAccessor contextAccessor, TicketingCommentService commentService)
    {
        _dbContext = dbContext;
        _contextAccessor = contextAccessor;
        _commentService = commentService;
    }

    public async Task<PagedResult<ListTicketDto>> ListOfTicket(TicketListFilter filters, CancellationToken cancellationToken = default)
    {
        var predicate = PredicateBuilder.New<Ticket>(true);

        if (filters.IsClosed.HasValue)
        {
            predicate = predicate.And(x => x.IsClosed == filters.IsClosed);
        }

        if (!string.IsNullOrEmpty(filters.Title))
        {
            predicate = predicate.And(x => x.Title.Contains(filters.Title));
        }
        
        if (filters.Number.HasValue)
        {
            predicate = predicate.And(x => x.Number == filters.Number);
        }
        
        if (filters.DepartmentId.HasValue)
        {
            predicate = predicate.And(x => x.DepartmentId == filters.DepartmentId);
        }
        
        if (filters.CategoryId.HasValue)
        {
            predicate = predicate.And(x => x.Categories.Any(c => c.CategoryId == filters.CategoryId));
        }
        
        if (filters.Users is { Count: > 0 })
        {
            predicate = predicate.And(x => x.Users.Any(u => filters.Users.Contains(u.UserId)));
        }

        if (!string.IsNullOrEmpty(filters.StatusName))
        {
            predicate = predicate.And(x => x.Status.Name == filters.StatusName);
        }
        
        if (filters.Priority.HasValue)
        {
            predicate = predicate.And(x => x.Priority == filters.Priority);
        }

        return await _dbContext.Tickets.AsExpandable()
            // .Include(x => x.Department)
            // .Include(x => x.Categories.AsQueryable().Where(c => c.Category.CreatorId == _contextAccessor.HttpContext!.User.GetUserId()))
            .Where(predicate)
            .Where(x => x.Users.Any(u => u.UserId == _contextAccessor.HttpContext!.User.GetUserId()))
            .Where(x => x.TenantId == _contextAccessor.HttpContext!.GetTenantId())
            .OrderByDescending(x => x.Id)
            .Select(TicketMapper.ListMapper(_contextAccessor.HttpContext!.User.GetUserId()))
            .SortAndPagedResultAsync(filters, cancellationToken);
    }

    public async Task<TicketDto> GetTicket(Guid ticketId, CancellationToken cancellationToken = default)
    {
        var ticket = await _dbContext.Tickets
            .Include(t => t.Users)
            .ThenInclude(tc => tc.User)
            
            .Include(x => x.Department)
            .Include(x => x.Creator)
            
            .Include(x => x.Comments)
            .ThenInclude(c => c.Creator)
            
            .Include(x => x.Comments)
            .ThenInclude(c => c.Files)

            .Include(x => x.Comments)
            .ThenInclude(c => c.Replay)
            .ThenInclude(cr => cr.Creator)
            
            .Include(t => t.Status)
            
            .Include(x => x.UserHistories)
            .ThenInclude(x => x.User)
            .Include(x => x.UserHistories)
            .ThenInclude(x => x.Creator)
            
            .Where(x => x.Id == ticketId)
            .Where(x => x.Users.Any(u => u.UserId == _contextAccessor.HttpContext!.User.GetUserId()))
            .Where(x => x.TenantId == _contextAccessor.HttpContext!.GetTenantId())
            .IgnoreQueryFilters() // add this to remove soft delete filter
            // .Select(TicketMapper.Mapper)
            .FirstOrDefaultAsync(cancellationToken);

        if (ticket == null)
        {
            throw new TicketNotFoundException();
        }

        var sql = string.Format("INSERT INTO \"TicketCommentSeens\" (\"CreatorId\", \"CommentId\", \"CreatedAt\")" +
                  " SELECT '{0}', TC.\"Id\", now() FROM \"TicketComments\" TC" +
        " LEFT JOIN \"TicketCommentSeens\" CC ON CC.\"CommentId\" = TC.\"Id\" AND CC.\"CreatorId\" = '{0}'" +
        " WHERE CC.\"CommentId\" IS NULL AND TC.\"TicketId\" = '{1}'", _contextAccessor.HttpContext!.User.GetUserId(), ticketId);

        await _dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken);

        foreach (var userHistory in ticket.UserHistories)
        {
            ticket.Comments.Add(new TicketComment
            {
                Id = userHistory.Id,
                Creator = userHistory.Creator,
                Message = userHistory.User.FullName ?? "",
                CreatedAt = userHistory.CreatedAt,
                Type = Enum.GetName(typeof(TicketUserHistoryType), userHistory.Type)!.ToLower(),
            });
        }

        ticket.Comments = ticket.Comments.OrderBy(x => x.CreatedAt).ToList();

        return ticket.Adapt<TicketDto>();
    }

    public async Task<Guid> CreateTicket(TicketCreateDto request, CancellationToken cancellationToken = default)
    {
        var newStatus = await _dbContext.TicketStatuses.FirstOrDefaultAsync(x => x.Name == "new", cancellationToken);

        if (newStatus == null)
        {
            throw new TicketStatusNotFoundException();
        }

        var number = await _dbContext.Tickets
            .Where(x => x.TenantId == _contextAccessor.HttpContext!.GetTenantId())
            .OrderByDescending(x => x.Number)
            .Select(x => x.Number)
            .FirstOrDefaultAsync(cancellationToken);

        var users = new List<Guid>();

        if (request.UserIds is { Count: > 0 })
        {
            users.AddRange(request.UserIds);
        }
        
        if (users.All(x => x != _contextAccessor.HttpContext!.User.GetUserId()))
        {
            users.Add(_contextAccessor.HttpContext!.User.GetUserId());
        }

        if (request.DepartmentId.HasValue)
        {
            var department = await _dbContext.Departments
                .Include(x => x.DepartmentUsers)
                .FirstAsync(x => x.Id == request.DepartmentId, cancellationToken);

            foreach (var departmentUser in department.DepartmentUsers)
            {
                if (users.All(x => x != departmentUser.UserId))
                {
                    users.Add(departmentUser.UserId);
                }
            }
        }
        
        var ticket = new Ticket()
        {
            Title = request.Title,
            Number = (number == 0 ? 1000 : number) + 1,
            DepartmentId = request.DepartmentId,
            Users = users.Select(x => new TicketUser() { UserId = x }).ToList(),
            UserHistories = users.Select(x => new TicketUserHistory { UserId = x, Type = TicketUserHistoryType.ADD }).ToList(),
            Status = newStatus,
            LastCommentAt = DateTime.Now,
            Priority = request.Priority ?? TicketPriority.Low
        };
        
        if (request.Categories is { Count: > 0 })
        {
            ticket.Categories = await _dbContext.Categories
                .Where(x => request.Categories.Contains(x.Id))
                .Select(x => new TicketCategory()
                {
                    CategoryId = x.Id
                })
                .ToListAsync(cancellationToken);
            
        }

        _dbContext.TicketStatusHistories.Add(new TicketStatusHistory()
        {
            Status = newStatus,
            Ticket = ticket
        });

        var comment = new TicketComment()
        {
            Message = request.Message
        };
        
        // comment.TicketCommentSeens.Add(new TicketCommentSeen()
        // {
        //     CreatorId = _contextAccessor.HttpContext!.User.GetUserId()
        // });
        
        ticket.Comments.Add(comment);

        if (request.DepartmentId.HasValue)
        {
            ticket.DepartmentHistories.Add(new TicketDepartmentHistory
            {
                DepartmentId = request.DepartmentId.Value
            });
        }
        
        comment.TicketCommentSeens.Add(new TicketCommentSeen()
        {
            CreatorId = _contextAccessor.HttpContext!.User.GetUserId()
        });

        await _dbContext.Tickets.AddAsync(ticket, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        if (request.Files != null)
        {
            await _commentService.SaveTicketCommentFiles(comment, request.Files, cancellationToken);
        }

        return ticket.Id; //await GetTicket(ticket.Id, cancellationToken);
    }

    public async Task UpdateTicket(TicketUpdateDto request, Guid ticketId, CancellationToken cancellationToken = default)
    {
        var ticket = await _dbContext.Tickets
            .Where(x => x.Id == ticketId)
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(cancellationToken);

        if (ticket == null)
        {
            throw new TicketNotFoundException();
        }

        ticket.Title = request.Title;
        _dbContext.Tickets.Update(ticket);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteTicket(Guid ticketId, CancellationToken cancellationToken = default)
    {
        var ticket = await _dbContext.Tickets
            .Where(x => x.TenantId == _contextAccessor.HttpContext!.GetTenantId())
            .Where(x => x.Id == ticketId)
            .FirstOrDefaultAsync(cancellationToken);

        if (ticket == null)
        {
            throw new TicketNotFoundException();
        }
        
        ticket.DeletedAt = DateTime.Now;
        ticket.DeletedBy = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == _contextAccessor.HttpContext!.User.GetUserId(), cancellationToken);
        _dbContext.Tickets.Update(ticket);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    /// <summary>
    /// Change ticket department.
    /// This method will be add department user to ticket if not exists.
    /// </summary>
    /// <param name="ticketId"></param>
    /// <param name="departmentId"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="TicketNotFoundException"></exception>
    public async Task MoveToDepartment(Guid ticketId, Guid departmentId, CancellationToken cancellationToken = default)
    {
        var ticket = await _dbContext.Tickets
            .Include(x => x.Users)
            .Include(x => x.DepartmentHistories)
            .Where(x => x.Id == ticketId)
            .Where(x => x.TenantId == _contextAccessor.HttpContext!.GetTenantId())
            .FirstOrDefaultAsync(cancellationToken);

        if (ticket == null)
        {
            throw new TicketNotFoundException();
        }

        if (ticket.DepartmentId == departmentId)
        {
            return;
        }
        
        var department = await _dbContext.Departments
            .Include(x => x.DepartmentUsers)
            .FirstAsync(x => x.Id == departmentId, cancellationToken);

        foreach (var departmentUser in department.DepartmentUsers)
        {
            if (ticket.Users.Any(x => x.UserId == departmentUser.UserId)) continue;
            
            ticket.Users.Add(new TicketUser
            {
                UserId = departmentUser.UserId
            });
                
            ticket.UserHistories.Add(new TicketUserHistory
            {
                UserId = departmentUser.UserId
            });
        }
        
        ticket.DepartmentId = departmentId;
        ticket.DepartmentHistories.Add(new TicketDepartmentHistory
        {
            DepartmentId = departmentId
        });
        
        _dbContext.Tickets.Update(ticket);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Assign categories to ticket by user. no body can see selected categories only user who assigned will see 
    /// </summary>
    /// <param name="ticketId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="TicketNotFoundException"></exception>
    public async Task AssignCategories(Guid ticketId, AssignTicketCategoriesDto request, CancellationToken cancellationToken = default)
    {
        var ticket = await _dbContext.Tickets
            .Where(x => x.Id == ticketId)
            .Where(x => x.TenantId == _contextAccessor.HttpContext!.GetTenantId())
            .FirstOrDefaultAsync(cancellationToken);

        if (ticket == null)
        {
            throw new TicketNotFoundException();
        }

        await _dbContext.TicketCategories.Where(x => x.Ticket.Id == ticketId && x.CategoryId == _contextAccessor.HttpContext!.User.GetUserId()).ExecuteDeleteAsync(cancellationToken);
        ticket.Categories = request.Categories.Select(x => new TicketCategory() { CategoryId = x }).ToList();
        
        _dbContext.Tickets.Update(ticket);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Get ticket histories of changes like users add/remove/left, status changes and etc.
    /// </summary>
    /// <param name="ticketId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ICollection<TicketHistoryDto>> GetHistories(Guid ticketId, CancellationToken cancellationToken = default)
    {
        ICollection<TicketHistoryDto> histories = new List<TicketHistoryDto>();

        var userHistories = await _dbContext.TicketUserHistories
            .Include(x => x.Creator)
            .Include(x => x.User)
            .Where(x => x.TicketId == ticketId)
            .IgnoreQueryFilters()
            .ToListAsync(cancellationToken);
        
        var commentHistories = await _dbContext.TicketComments
            .Include(x => x.Creator)
            .Include(x => x.DeletedBy)
            .Where(x => x.Ticket.Id == ticketId)
            .IgnoreQueryFilters()
            .ToListAsync(cancellationToken);

        var statusHistories = await _dbContext.TicketStatusHistories
            .Include(x => x.Status)
            .Include(x => x.Creator)
            .Where(x => x.Ticket.Id == ticketId)
            .ToListAsync(cancellationToken);

        var departmentHistories = await _dbContext.TicketDepartmentHistories
            .Include(x => x.Creator)
            .Include(x => x.Department)
            .Where(x => x.TicketId == ticketId)
            .ToListAsync(cancellationToken);

        foreach (var userHistory in userHistories)
        {
            var message = "";
            switch (userHistory.Type)
            {
                case TicketUserHistoryType.ADD:
                    message = string.Format("اضافه کردن {0} توسط {1}", userHistory.User.FullName, userHistory.Creator.FullName);
                    break;
                case TicketUserHistoryType.DELETE:
                    message = string.Format("حذف {0} توسط {1}", userHistory.User.FullName, userHistory.Creator.FullName);
                    break;
                case TicketUserHistoryType.LEFT:
                    message = string.Format("ترک تیکت توسط {0}", userHistory.User.FullName);
                    break;
                default:
                    break;
            }
            
            histories.Add(new TicketHistoryDto()
            {
                Message = message,
                CreatedAt = userHistory.CreatedAt
            });
        }

        foreach (var commentHistory in commentHistories)
        {
            histories.Add(new TicketHistoryDto()
            {
                Message = string.Format("ارسال پیام توسط {0}", commentHistory.Creator.FullName),
                CreatedAt = commentHistory.CreatedAt
            });

            if (commentHistory.DeletedAt != null)
            {
                histories.Add(new TicketHistoryDto()
                {
                    Message = string.Format("حذف پیام توسط {0}", commentHistory.DeletedBy.FullName),
                    CreatedAt = commentHistory.CreatedAt
                });
            }
        }

        foreach (var statusHistory in statusHistories)
        {
            histories.Add(new TicketHistoryDto()
            {
                Message = string.Format("تغییر وضعیت تیکت به {0} توسط {1}", statusHistory.Status.Title, statusHistory.Creator.FullName),
                CreatedAt = statusHistory.CreatedAt
            });
        }

        foreach (var departmentHistory in departmentHistories)
        {
            histories.Add(new TicketHistoryDto
            {
                Message = string.Format("اضافه کردن دپارتمان {0} توسط {1}", departmentHistory.Department.Title, departmentHistory.Creator.FullName),
                CreatedAt = departmentHistory.CreatedAt,
            });
        }
        
        return histories.OrderBy(x => x.CreatedAt).ToList();
    }

    public async Task<TicketStatusDto> ChangeStatus(Guid ticketId, TicketChangeStatusRequest request, CancellationToken cancellationToken = default)
    {
        var ticket = await _dbContext.Tickets
            .Where(x => x.Id == ticketId)
            .FirstOrDefaultAsync(cancellationToken);

        if (ticket == null)
        {
            throw new TicketNotFoundException();
        }

        var status = await _dbContext.TicketStatuses.FirstOrDefaultAsync(x => x.Name == request.StatusName, cancellationToken);

        if (status == null)
        {
            throw new TicketStatusNotFoundException();
        }
        
        _dbContext.TicketStatusHistories.Add(new TicketStatusHistory
        {
            Status = status,
            Ticket = ticket
        });

        ticket.Status = status;

        _dbContext.Tickets.Update(ticket);

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return status.Adapt<TicketStatusDto>();
    }
}