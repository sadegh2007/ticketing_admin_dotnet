namespace ERP.Ticketing.HttpApi.Services.Smslr;

public class TokenResponse
{
    public string? TokenKey { get; set; } = null;
    public bool IsSuccessful { get; set; }
    public string Message { get; set; } = "";
}