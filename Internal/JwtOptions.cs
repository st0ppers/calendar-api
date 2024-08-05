namespace CalendarApi.Internal;

public sealed class JwtOptions
{
    public const string Section = "JwtOptions";
    public string SigningKey { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int ExpirationSeconds { get; set; }
}


