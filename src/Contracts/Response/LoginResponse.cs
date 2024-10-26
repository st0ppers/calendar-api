namespace CalendarApi.Contracts.Response;

public sealed class LoginResponse
{
    public PlayerResponse Player { get; init; }
    public string Token { get; init; } = string.Empty;
    public int Expiration { get; init; }
}