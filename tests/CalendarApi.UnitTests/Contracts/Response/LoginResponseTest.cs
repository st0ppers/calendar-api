using CalendarApi.Contracts.Response;
using FluentAssertions;

namespace CalendarApi.UnitTests.Contracts.Response;

public class LoginResponseTest
{
    [Test]
    public void LoginResponse_ShouldInitializeCorrectly()
    {
        // Arrange 
        const string token = "token";
        const string username = "username";
        const string id = "1";
        const string color = "blue";
        const int expiration = 123;
        var player = new PlayerResponse { Id = id, Username = username, Color = color };

        //Act
        var loginResponse = new LoginResponse { Token = token, Expiration = expiration, Player = player };

        // Assert
        loginResponse.Should().NotBeNull();
        loginResponse.Token.Should<string>().NotBe(string.Empty);
        loginResponse.Expiration.Should<int>().NotBe(default);
        loginResponse.Player.Should().NotBeNull().Should().NotBe(default);

        loginResponse.Token.Should<string>().Be(token);
        loginResponse.Expiration.Should<int>().Be(expiration);
        loginResponse.Player.Should().Be(player);
    }
}