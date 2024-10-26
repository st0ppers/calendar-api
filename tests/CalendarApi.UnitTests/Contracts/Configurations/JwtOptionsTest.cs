using CalendarApi.Contracts.Configurations;
using FluentAssertions;

namespace CalendarApi.UnitTests.Contracts.Configurations;

public class JwtOptionsTest
{
    [Test]
    public void JwtOptions_InitializeCorrectly()
    {
        // Arrange
        const string signingKey = "test";
        const string issuer = "test";
        const string audience = "test";
        const int expirationSeconds = 123;

        // Act
        var jwtOptions = new JwtOptions
        {
            SigningKey = signingKey,
            Issuer = issuer,
            Audience = audience,
            ExpirationSeconds = expirationSeconds
        };

        // Assert
        jwtOptions.Should().NotBe(null);
        jwtOptions.SigningKey.Should<string>().NotBe(string.Empty);
        jwtOptions.Issuer.Should<string>().NotBe(string.Empty);
        jwtOptions.Audience.Should<string>().NotBe(string.Empty);
        jwtOptions.ExpirationSeconds.Should<int>().NotBe(default);
        
        jwtOptions.SigningKey.Should<string>().Be(signingKey);
        jwtOptions.Issuer.Should<string>().Be(issuer);
        jwtOptions.Audience.Should<string>().Be(audience);
        jwtOptions.ExpirationSeconds.Should<int>().Be(expirationSeconds);
    }
}