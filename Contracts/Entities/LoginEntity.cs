namespace CalendarApi.Contracts.Entities;

public sealed record LoginEntity
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}