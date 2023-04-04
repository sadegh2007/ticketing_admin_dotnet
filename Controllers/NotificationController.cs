using ERP.Shared.Commons;
using ERP.Shared.Notifications;
using ERP.Ticketing.HttpApi.Controllers.Common;
using ERP.Ticketing.HttpApi.Features.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Ticketing.HttpApi.Controllers;

public class NotificationController: AuthApiControllerBase
{
    private readonly NotificationService _notificationService;

    public NotificationController(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost("list", Name = "notifications_list")]
    public async Task<PagedResult<NotificationDto>> List(NotificationFilter filters)
    {
        return await _notificationService.ListOfNotifications(filters);
    }
}