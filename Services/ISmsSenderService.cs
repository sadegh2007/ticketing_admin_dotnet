namespace ERP.Ticketing.HttpApi.Services;

public interface ISmsSenderService
{
    Task SendAsync(string mobile, string message, CancellationToken cancellationToken = default);

    public Task SendLookupAsync(string mobile, string token, string lookupTemplate, string lookupType = "sms",
        CancellationToken cancellationToken = default);
}