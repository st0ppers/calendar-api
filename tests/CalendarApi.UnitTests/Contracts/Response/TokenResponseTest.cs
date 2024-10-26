using CalendarApi.Contracts;
using CalendarApi.Contracts.Response;
using FluentAssertions;

namespace CalendarApi.UnitTests.Contracts.Response;

public class TokenResponseTest
{
    [Test]
    public void TokenResponse_ShouldInitializeCorrectly()
    {
        // Arrange 
        const string token = "token";
        const int expiration = 9;

        //Act
        var tokenResponse = new TokenResponse { AccessToken = token, Expiration = expiration };

        // Assert
        tokenResponse.Should().NotBeNull();
        tokenResponse.AccessToken.Should<string>().NotBe(string.Empty);
        tokenResponse.Expiration.Should<int>().NotBe(0);

        tokenResponse.AccessToken.Should<string>().Be(token);
        tokenResponse.Expiration.Should<int>().Be(expiration);
    }
}