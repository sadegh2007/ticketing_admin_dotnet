using ERP.Shared.Notifications;
using ERP.Shared.Ticketing;
using ERP.Ticketing.HttpApi.Commons.Exceptions;
using ERP.Ticketing.HttpApi.Commons.Extensions;
using ERP.Ticketing.HttpApi.Commons.Hubs;
using ERP.Ticketing.HttpApi.Data;
using ERP.Ticketing.HttpApi.Features.Notifications;
using ERP.Ticketing.HttpApi.Features.Ticketing.Mappers;
using ERP.Ticketing.HttpApi.Features.Users;
using LinqKit;
using Mapster;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ERP.Ticketing.HttpApi.Features.Ticketing;

public class TicketingCommentService
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IWebHostEnvironment _environment;
    private readonly IHubContext<MessageHub> _hubContext;
    private readonly UserConnectionService _userConnectionService;
    private readonly NotificationService _notificationService;

    public TicketingCommentService(AppDbContext dbContext,
        IHttpContextAccessor contextAccessor,
        IWebHostEnvironment environment,
        IHubContext<MessageHub> hubContext,
        UserConnectionService userConnectionService,
        NotificationService notificationService)
    {
        _dbContext = dbContext;
        _contextAccessor = contextAccessor;
        _environment = environment;
        _hubContext = hubContext;
        _userConnectionService = userConnectionService;
        _notificationService = notificationService;
    }

    public async Task<TicketCommentDto> CreateComment(Guid ticketId, TicketCommentCreateDto request,
        CancellationToken cancellationToken = default)
    {
        var ticket = await _dbContext.Tickets
            .Include(x => x.Users)
            .Include(x => x.Comments)
            .FirstOrDefaultAsync(x => x.Id == ticketId, cancellationToken);

        if (ticket == null)
        {
            throw new TicketNotFoundException();
        }

        var comment = new TicketComment()
        {
            Message = request.Message,
            ReplayId = request.ReplayId,
            Ticket = ticket,
        };

        comment.TicketCommentSeens.Add(new TicketCommentSeen()
        {
            CreatorId = _contextAccessor.HttpContext!.User.GetUserId()
        });

        _dbContext.TicketComments.Add(comment);
        
        ticket.LastCommentAt = DateTime.Now;
        _dbContext.Tickets.Update(ticket);
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        if (request.Files != null)
        {
            comment = await SaveTicketCommentFiles(comment, request.Files, cancellationToken);
        }

        var connectionIds = _userConnectionService.GetConnectionIds(ticket.Users.Select(x => x.UserId).ToList());

        var notificationMessage = string.Format("پیام جدید در تیکت {0}", ticket.Title);

        comment = await _dbContext.TicketComments
            .Include(x => x.Creator)
            .Include(x => x.Files)
            .Include(x => x.Replay)
            .ThenInclude(x => x.Creator)
            .FirstAsync(
                x => x.Id == comment.Id,
                cancellationToken
            );

        foreach (var user in ticket.Users.Where(x => x.UserId != _contextAccessor.HttpContext.User.GetUserId()).ToList())
        {
            await _notificationService.CreateAsync(
                NotificationType.NewComment,
                user.UserId,
                notificationMessage,
                new
                {
                    TicketId = ticket.Id,
                    CommentId = comment.Id
                },
                cancellationToken);
        }
        
        if (connectionIds.Count <= 0) return comment.Adapt<TicketCommentDto>();

        await _hubContext.Clients.Clients(connectionIds)
            .SendAsync("ReceiveMessage", notificationMessage, cancellationToken: cancellationToken);

        return comment.Adapt<TicketCommentDto>();
    }

    public async Task DeleteComment(Ticket ticket, Guid commentId, CancellationToken cancellationToken = default)
    {
        var comment = await _dbContext.TicketComments
            .FirstOrDefaultAsync(x => x.Id == commentId && x.Ticket.Id == ticket.Id, cancellationToken);

        if (comment == null)
        {
            throw new TicketCommentNotFoundException();
        }

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Id == _contextAccessor.HttpContext!.User.GetUserId(), cancellationToken);

        comment.DeletedAt = DateTime.Now;
        comment.DeletedBy = user;

        _dbContext.TicketComments.Update(comment);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<TicketComment> SaveTicketCommentFiles(TicketComment comment, IEnumerable<IFormFile> files,
        CancellationToken cancellationToken = default)
    {
        foreach (var file in files)
        {
            if (file.Length == 0)
            {
                continue;
            }

            var tempFile = CreateTempFilePath(comment, file.FileName);
            await using (var stream = File.OpenWrite(tempFile))
            {
                await file.CopyToAsync(stream, cancellationToken);
            }

            comment.Files.Add(new TicketCommentFile
            {
                FileName = file.FileName,
                Path = tempFile.Replace(_environment.WebRootPath, ""), // remove full path
                Extension = Path.GetExtension(file.FileName),
                Size = file.Length
            });
        }

        _dbContext.TicketComments.Update(comment);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return comment;
    }

    public async Task<IList<TicketCommentSeenDto>> CommentViewers(Guid ticketId, Guid commentId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.TicketCommentSeens
            .AsExpandable()
            .Include(x => x.Creator)
            .Where(x => x.Comment.Id == commentId && x.Comment.Ticket.Id == ticketId)
            .Select(TicketCommentViewerMapper.Mapper)
            .ToListAsync(cancellationToken);
    }

    private string CreateTempFilePath(TicketComment comment, string fileName)
    {
        var directoryPath = Path.Combine(_environment.WebRootPath, $"uploads/ticket_comments/{comment.Id}");
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

        return Path.Combine(directoryPath, fileName);
    }
}