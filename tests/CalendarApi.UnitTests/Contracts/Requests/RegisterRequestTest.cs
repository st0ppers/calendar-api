using CalendarApi.Contracts.Requests;
using FluentAssertions;

namespace CalendarApi.UnitTests.Contracts.Requests;

public class RegisterRequestTest
{
    [Test]
    public void RegisterRequest_ShouldInitializeCorrectly()
    {
        // Arrange 
        const string username = "username";
        const string password = "password";
        const string color = "blue";

        //Act
        var registerRequest = new RegisterRequest { Username = username, Password = password, Color = color };

        // Assert
        registerRequest.Should().NotBeNull();
        registerRequest.Username.Should<string>().NotBe(string.Empty);
        registerRequest.Password.Should<string>().NotBe(string.Empty);
        registerRequest.Color.Should<string>().NotBe(string.Empty);

        registerRequest.Username.Should<string>().Be(username);
        registerRequest.Password.Should<string>().Be(password);
        registerRequest.Color.Should<string>().Be(color);
    }
}