using CalendarApi.Contracts.Requests;
using FluentAssertions;

namespace CalendarApi.UnitTests.Contracts.Requests;

public class LoginRequestTest
{
    [Test]
    public void LoginRequest_ShouldInitializeCorrectly()
    {
        // Arrange 
        const string username = "username";
        const string password = "password";

        //Act
        var loginRequest = new LoginRequest { Username = username, Password = password };

        // Assert
        loginRequest.Should().NotBeNull();
        loginRequest.Username.Should<string>().NotBe(string.Empty);
        loginRequest.Password.Should<string>().NotBe(string.Empty);

        loginRequest.Username.Should<string>().Be(username);
        loginRequest.Password.Should<string>().Be(password);
    }
}