using CalendarApi.Contracts.Configurations;
using FluentAssertions;

namespace CalendarApi.UnitTests.Contracts.Configurations;

public class ConnectionStringTest
{
    [Test]
    public void Test_Default()
    {
        // Arrange
        const string defaultString = "default";

        // Act
        var connectionString = new ConnectionString { Default = defaultString };

        // Assert
        connectionString.Should().NotBeNull();
        connectionString.Default.Should<string>().NotBeNull();
        connectionString.Default.Should<string>().NotBe(string.Empty);
        connectionString.Default.Should<string>().Be(defaultString);
        
    }
}