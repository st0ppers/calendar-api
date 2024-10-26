namespace CalendarApi.Contracts.Requests;

public sealed class RegisterRequest
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Color { get; init; } = string.Empty;
}