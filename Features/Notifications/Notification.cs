using System.Text.Json;
using System.Text.Json.Nodes;
using ERP.Shared.Notifications;
using ERP.Ticketing.HttpApi.Data.Common.Helpers;
using ERP.Ticketing.HttpApi.Features.Users;

namespace ERP.Ticketing.HttpApi.Features.Notifications;

public class Notification: IModel, ICreateTime
{
    public Guid Id { get; set; }
    public NotificationType Type { get; set; }
    public string Title { get; set; }
    public JsonDocument? Data { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime CreatedAt { get; set; }
}