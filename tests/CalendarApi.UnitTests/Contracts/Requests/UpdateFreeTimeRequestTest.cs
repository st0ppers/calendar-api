using CalendarApi.Contracts.Requests;
using FluentAssertions;

namespace CalendarApi.UnitTests.Contracts.Requests;

public class UpdateFreeTimeRequestTest
{
    [Test]
    public void UpdateFreeTimeRequest_ShouldInitializeCorrectly()
    {
        //Arrange
        var playerId = "playerId";
        var from = DateTime.Now;
        var to = DateTime.Now.AddDays(1);

        //Act
        var updateFreeTimeRequest = new UpdateFreeTimeRequest { PlayerId = playerId, From = from, To = to };
        
        //Assert
        updateFreeTimeRequest.Should().NotBeNull();
        updateFreeTimeRequest.PlayerId.Should<string>().NotBe(string.Empty);
        updateFreeTimeRequest.From.Should<DateTime>().NotBe(default);
        updateFreeTimeRequest.To.Should<DateTime>().NotBe(default);
        
        updateFreeTimeRequest.PlayerId.Should<string>().Be(playerId);
        updateFreeTimeRequest.From.Should<DateTime>().Be(from);
        updateFreeTimeRequest.To.Should<DateTime>().Be(to);
    }
}