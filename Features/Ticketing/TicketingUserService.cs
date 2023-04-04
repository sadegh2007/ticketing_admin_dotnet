using ERP.Shared.Ticketing;
using ERP.Ticketing.HttpApi.Commons.Exceptions;
using ERP.Ticketing.HttpApi.Commons.Extensions;
using ERP.Ticketing.HttpApi.Data;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace ERP.Ticketing.HttpApi.Features.Ticketing;

public class TicketingUserService
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _contextAccessor;

    public TicketingUserService(AppDbContext dbContext, IHttpContextAccessor contextAccessor)
    {
        _dbContext = dbContext;
        _contextAccessor = contextAccessor;
    }

    public async Task AddUsers(TicketUserAddDto request, Guid ticketId, CancellationToken cancellationToken = default)
    {
        var ticket = await _dbContext.Tickets.SingleOrDefaultAsync(x => x.Id == ticketId, cancellationToken);

        if (ticket == null)
        {
            throw new TicketNotFoundException();
        }

        var usersInTicket = await _dbContext.TicketUsers
            .Include(x => x.User)
            .Where(x => x.Ticket.Id == ticketId)
            .Where(x => request.UserIds.Contains(x.UserId))
            .ToListAsync(cancellationToken);
        
        if (usersInTicket.Count > 0)
        {
            throw new TicketUserAlreadyAddedException($@"{usersInTicket.First().User.Name} قبلا اضافه شده است.");
        }
        
        foreach (var userId in request.UserIds)
        {
            ticket.Users.Add(new TicketUser()
            {
                UserId = userId,
                Ticket = ticket
            });
            
            ticket.UserHistories.Add(new TicketUserHistory()
            {
                UserId = userId,
                Ticket = ticket,
                Type = TicketUserHistoryType.ADD
            });
        }

        await  _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveUser(Guid ticketId, Guid userId, CancellationToken cancellationToken = default)
    {
        var ticket = await _dbContext.Tickets.SingleOrDefaultAsync(x => x.Id == ticketId, cancellationToken);

        if (ticket == null)
        {
            throw new TicketNotFoundException();
        }

        var ticketUser = await _dbContext.TicketUsers.FirstOrDefaultAsync(
            x => x.UserId == userId && x.Ticket.Id == ticketId, 
            cancellationToken
            );

        if (ticketUser == null)
        {
            throw new TicketUserNotExistsException();
        }

        var type = TicketUserHistoryType.DELETE;

        // SET TYPE TO LEFT WHEN CURRENT USER IS DELETE USER
        if (_contextAccessor.HttpContext!.User.GetUserId() == ticketUser.UserId)
        {
            type = TicketUserHistoryType.LEFT;
        }

        _dbContext.TicketUsers.Remove(ticketUser);
        _dbContext.TicketUserHistories.Add(new TicketUserHistory()
        {
            UserId = ticketUser.UserId,
            Type = type,
            TicketId = ticketId
        });
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}