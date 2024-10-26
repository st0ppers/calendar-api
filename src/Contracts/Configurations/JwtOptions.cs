namespace CalendarApi.Contracts.Configurations;

public sealed class JwtOptions
{
    public const string Section = "JwtOptions";
    public string SigningKey { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public int ExpirationSeconds { get; init; }
}


