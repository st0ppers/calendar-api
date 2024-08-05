namespace CalendarApi.Internal;

public sealed class ConnectionString
{
    public const string Section = "ConnectionStrings";
    public string Default { get; init; } = null!;
}
