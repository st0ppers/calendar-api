using CalendarApi.Contracts;
using FluentAssertions;

namespace CalendarApi.UnitTests.Contracts;

public class FreeTimeTest
{
    [Test]
    public void FreeTime_ShouldInitializeCorrectly()
    {
        // Arrange 
        var from = DateTime.Now;
        var to = DateTime.Now.AddDays(1);

        //Act
        var freeTime = new FreeTime { From = from, To = to };

        // Assert
        freeTime.Should().NotBeNull();
        freeTime.From.Should<DateTime>().NotBe(default);
        freeTime.To.Should<DateTime>().NotBe(default);
        
        freeTime.From.Should<DateTime>().Be(from);
        freeTime.To.Should<DateTime>().Be(to);
    }
}