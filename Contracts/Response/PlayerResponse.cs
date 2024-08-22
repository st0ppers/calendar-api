namespace CalendarApi.Contracts.Response;

public sealed class PlayerResponse
{
    public string Id { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string Color { get; init; } = string.Empty;
    public FreeTime FreeTime { get; init; } = new();
}

