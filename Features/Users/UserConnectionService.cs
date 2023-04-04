using ERP.Ticketing.HttpApi.Commons.Extensions;

namespace ERP.Ticketing.HttpApi.Features.Users;

public class UserConnectionService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private IDictionary<Guid, string> _connections { get; set; }

    public UserConnectionService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _connections = new Dictionary<Guid, string>();
    }

    public string? GetConnectionId(Guid userId)
    {
        return _connections.ContainsKey(userId) ? _connections[userId] : null;
    }
    
    public ICollection<string> GetConnectionIds(ICollection<Guid> userIds)
    {
        return _connections
            .Where(x => x.Key != _httpContextAccessor.HttpContext!.User.GetUserId())
            .Where(x => userIds.Contains(x.Key))
            .Select(x => x.Value).ToList();
    }

    public void AddUser(Guid userId, string connectionId)
    {
        if (_connections.ContainsKey(userId))
        {
            _connections[userId] = connectionId;
        }
        else
        {
            _connections.Add(userId, connectionId);
        }
    }
    
    public void RemoveUser(Guid userId)
    {
        if (_connections.ContainsKey(userId))
        {
            _connections.Remove(userId);
        }
    }
}