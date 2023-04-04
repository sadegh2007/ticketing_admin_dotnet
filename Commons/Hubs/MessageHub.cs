using ERP.Ticketing.HttpApi.Commons.Extensions;
using ERP.Ticketing.HttpApi.Features.Users;
using Microsoft.AspNetCore.SignalR;

namespace ERP.Ticketing.HttpApi.Commons.Hubs;

public class MessageHub: Hub
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly UserConnectionService _connectionService;

    public MessageHub(IHttpContextAccessor contextAccessor, UserConnectionService connectionService)
    {
        _contextAccessor = contextAccessor;
        _connectionService = connectionService;
    }

    public async Task SendToOthersAsync(ICollection<Guid> userIds, string message)
    {
        var connectionIds = _connectionService.GetConnectionIds(userIds);

        if (connectionIds.Count > 0)
        {
            await Clients.Clients(connectionIds).SendAsync("ReceiveMessage", message);
        }
    }

    public override Task OnConnectedAsync()
    {
        _connectionService.AddUser(_contextAccessor.HttpContext!.User.GetUserId(), Context.ConnectionId);
        return base.OnConnectedAsync();
    }
}