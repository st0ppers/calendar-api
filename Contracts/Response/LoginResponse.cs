namespace CalendarApi.Contracts.Response;

public sealed class LoginResponse
{
    public PlayerResponse Player { get; set; }
    public string Token { get; set; }
    public int Expiration { get; set; }
}