namespace ERP.Ticketing.HttpApi.Services.Smslr;

public class SmsSenderService: ISmsSenderService
{
    private readonly HttpClient _httpClient;
    private const string Token = "4E4837536E41715837572B434F495338704A2B457A71453345776C5A32773558636F4458723970696544513D";

    public SmsSenderService()
    {
        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://api.kavenegar.com/v1/")
        };
    }

    public async Task SendAsync(string mobile, string message, CancellationToken cancellationToken = default)
    {
        var data = new
        {
            receptor = mobile,
            message = message
        };

        var content = JsonContent.Create(data);
        content.Headers.TryAddWithoutValidation("Content-Type", "text/html");
        content.Headers.TryAddWithoutValidation("charset", "utf-8");

        var response = await _httpClient.PostAsync($"{Token}/sms/send.json", content, cancellationToken: cancellationToken);
        
        response.EnsureSuccessStatusCode();
    }

    public async Task SendLookupAsync(string mobile, string token, string lookupTemplate, string lookupType = "sms", CancellationToken cancellationToken = default)
    {
        var data = new
        {
            receptor = mobile,
            token = token,
            template = lookupTemplate,
            type = lookupType
        };

        var content = JsonContent.Create(data);
        // content.Headers.TryAddWithoutValidation("Content-Type", "text/html");
        // content.Headers.TryAddWithoutValidation("charset", "utf-8");

        var response = await _httpClient.PostAsync($"{Token}/verify/lookup.json", content, cancellationToken: cancellationToken);
        
        Console.WriteLine(await response.RequestMessage.Content.ReadAsStringAsync(cancellationToken));
        
        //response.EnsureSuccessStatusCode();
    }
}