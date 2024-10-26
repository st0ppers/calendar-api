namespace CalendarApi.Contracts.Entities;

public sealed class UpdateFreeTimeEntity
{
    public string PlayerId { get; init; } = string.Empty;
    public DateTime From { get; init; }
    public DateTime To { get; init; }
}

