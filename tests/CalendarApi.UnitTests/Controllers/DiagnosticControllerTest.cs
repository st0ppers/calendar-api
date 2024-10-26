using System.Text.Json;
using CalendarApi.Controllers;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using ConfigurationProvider = CalendarApi.UnitTests.Helpers.ConfigurationProvider;

namespace CalendarApi.UnitTests.Controllers;

public class DiagnosticControllerTest
{
    private DiagnosticsController _sut;
    
    [SetUp]
    public void Setup() => _sut = new DiagnosticsController(ConfigurationProvider.Configuration);
    
    [Test]
    public void GetConfigs_WhenCalled_ShouldReturnConfigs()
    {
        // Arrange
        var expected = ConfigurationProvider.Configuration.AsEnumerable().ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        
        // Act
        var result = _sut.GetConfigs();
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(JsonSerializer.Serialize(expected));
    }
}