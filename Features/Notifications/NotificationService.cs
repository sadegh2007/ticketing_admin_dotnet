using System.Text.Json;
using System.Text.Json.Nodes;
using ERP.Shared.Commons;
using ERP.Shared.Notifications;
using ERP.Ticketing.HttpApi.Commons.Extensions;
using ERP.Ticketing.HttpApi.Data;
using ERP.Ticketing.HttpApi.Features.Notifications.Mappers;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace ERP.Ticketing.HttpApi.Features.Notifications;

public class NotificationService
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _contextAccessor;

    public NotificationService(AppDbContext dbContext, IHttpContextAccessor contextAccessor)
    {
        _dbContext = dbContext;
        _contextAccessor = contextAccessor;
    }

    public async Task<PagedResult<NotificationDto>> ListOfNotifications(NotificationFilter filters,
        CancellationToken cancellationToken = default)
    {
        var predicate = PredicateBuilder.New<Notification>(true);

        var result = await _dbContext.Notifications.AsExpandable()
            .Include(x => x.User)
            .Where(predicate)
            .Where(x => x.UserId == _contextAccessor.HttpContext!.User.GetUserId())
            .OrderByDescending(x => x.CreatedAt)
            .OrderBy(x => x.ReadAt == null)
            .Select(NotificationMapper.Mapper)
            .SortAndPagedResultAsync(filters, cancellationToken);

        await ReadAll(cancellationToken);

        return result;
    }

    public async Task CreateAsync(NotificationType type, Guid userId, string title, Object? data,
        CancellationToken cancellationToken = default)
    {
        _dbContext.Notifications.Add(new Notification
        {
            Type = type,
            Title = title,
            UserId = userId,
            Data = JsonSerializer.SerializeToDocument(data)
        });

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ReadAll(CancellationToken cancellationToken = default)
    {
        _dbContext.Notifications
            .Where(x => x.ReadAt == null)
            .ExecuteUpdate(notif => notif.SetProperty(
                    n => n.ReadAt,
                    n => DateTime.UtcNow
                    ));

        // var notifications =  await _dbContext.Notifications.Where(x => x.ReadAt == null)
        //     .ToListAsync(cancellationToken);
        //
        // var newItems = new List<Notification>();
        //
        // foreach (var notification in notifications)
        // {
        //     notification.ReadAt = DateTime.Now;
        //     newItems.Add(notification);
        // }
        //
        // _dbContext.Notifications.UpdateRange(newItems);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}