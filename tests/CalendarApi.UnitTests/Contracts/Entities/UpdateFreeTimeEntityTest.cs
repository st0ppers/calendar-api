using CalendarApi.Contracts.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Web;

namespace CalendarApi.UnitTests.Contracts.Entities;

public class UpdateFreeTimeEntityTest
{
    [Test]
    public void UpdateFreeTimeEntity_ShouldInitializeCorrectly()
    {
        // Arrange 
        var from = DateTime.Now;
        var to = DateTime.Now.AddDays(1);
        var playerId = "1";

        //Act
        var updateFreeTimeEntity = new UpdateFreeTimeEntity { From = from, To = to, PlayerId = playerId };

        // Assert
        updateFreeTimeEntity.Should().NotBeNull();
        updateFreeTimeEntity.From.Should<DateTime>().NotBe(default);
        updateFreeTimeEntity.To.Should<DateTime>().NotBe(default);
        updateFreeTimeEntity.PlayerId.Should<string>().NotBe(string.Empty);

        updateFreeTimeEntity.From.Should<DateTime>().Be(from);
        updateFreeTimeEntity.To.Should<DateTime>().Be(to);
        updateFreeTimeEntity.PlayerId.Should<string>().Be(playerId);
    }
}