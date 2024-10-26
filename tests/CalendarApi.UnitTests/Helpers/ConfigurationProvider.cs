using Microsoft.Extensions.Configuration;

namespace CalendarApi.UnitTests.Helpers;

public static class ConfigurationProvider
{
    private const string AppSettingsJson = "appsettings.json";

    public static IConfiguration Configuration => new ConfigurationBuilder()
        .AddJsonFile(AppSettingsJson)
        .Build();
}