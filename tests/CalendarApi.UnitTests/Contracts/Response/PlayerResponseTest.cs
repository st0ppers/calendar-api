using CalendarApi.Contracts;
using CalendarApi.Contracts.Response;
using FluentAssertions;

namespace CalendarApi.UnitTests.Contracts.Response;

public class PlayerResponseTest
{
    [Test]
    public void LoginResponse_ShouldInitializeCorrectly()
    {
        // Arrange 
        const string username = "username";
        const string id = "1";
        const string color = "blue";
        var from = DateTime.Now;
        var to = DateTime.Now.AddDays(1);
        var freeTime = new FreeTime() { From = from, To = to };

        //Act
        var player = new PlayerResponse { Id = id, Username = username, Color = color, FreeTime = freeTime };

        // Assert
        player.Should().NotBeNull();
        player.Id.Should<string>().NotBe(string.Empty);
        player.Username.Should<string>().NotBe(string.Empty);
        player.Color.Should<string>().NotBe(string.Empty);
        player.FreeTime.Should().NotBe(null);
        player.FreeTime.Should().NotBe(default);

        player.Id.Should<string>().Be(id);
        player.Username.Should<string>().Be(username);
        player.Color.Should<string>().Be(color);
        player.FreeTime.Should().Be(freeTime);
    }
}