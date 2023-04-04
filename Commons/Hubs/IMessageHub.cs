namespace ERP.Ticketing.HttpApi.Commons.Hubs;

public interface IMessageHub
{
    public Task SendToOtherUsersAsync(ICollection<Guid> userIds, string message);
}