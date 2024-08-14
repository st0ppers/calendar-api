namespace CalendarApi.Contracts.Response;

public sealed class TokenResponse
{
    public string AccessToken { get; init; }

    public int Expiration { get; init; }
    // public string Type { get; set; } = "bearer";
}