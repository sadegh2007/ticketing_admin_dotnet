using System.Linq.Expressions;
using ERP.Shared.Notifications;
using ERP.Ticketing.HttpApi.Features.Users.Mappers;
using LinqKit;

namespace ERP.Ticketing.HttpApi.Features.Notifications.Mappers;

public static class NotificationMapper
{
    public static Expression<Func<Notification, NotificationDto>> Mapper = notification => new NotificationDto()
    {
        Id = notification.Id,
        Type = notification.Type,
        Title = notification.Title,
        UserId = notification.UserId,
        User = UserMapper.Mapper.Invoke(notification.User),
        Data = notification.Data,
        ReadAt = notification.ReadAt,
        CreatedAt = notification.CreatedAt
    };
}