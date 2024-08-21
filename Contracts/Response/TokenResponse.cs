namespace CalendarApi.Contracts.Response;

public sealed class TokenResponse
{
    public string AccessToken { get; init; } = string.Empty;
    public int Expiration { get; init; }
}