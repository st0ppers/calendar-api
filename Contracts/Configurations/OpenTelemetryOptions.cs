namespace CalendarApi.Contracts.Configurations;

public class OpenTelemetryOptions
{
    public const string Section = "OpenTelemetry";
    public string Endpoint { get; set; }
    public string ApiKey { get; set; }
}