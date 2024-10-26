using CalendarApi.Contracts.Configurations;
using FluentAssertions;

namespace CalendarApi.UnitTests.Contracts.Configurations;

public class OpenTelemetryOptionsTest
{
    [Test]
    public void OpenTelemetryOptions_ShouldInitializeCorrectly()
    {
        // Arrange 
        const string endpoint = "endpoint";
        const string apiKey = "apikey";

        //Act
        var openTelemetryOptions = new OpenTelemetryOptions { Endpoint = endpoint, ApiKey = apiKey };

        // Assert
        openTelemetryOptions.Should().NotBeNull();
        openTelemetryOptions.Endpoint.Should<string>().NotBe(string.Empty);
        openTelemetryOptions.ApiKey.Should<string>().NotBe(string.Empty);

        openTelemetryOptions.Endpoint.Should<string>().Be(endpoint);
        openTelemetryOptions.ApiKey.Should<string>().Be(apiKey);
    }
}