using CalendarApi.Contracts;
using CalendarApi.Contracts.Entities;
using FluentAssertions;

namespace CalendarApi.UnitTests.Contracts.Entities;

public class PlayerEntityTest
{
    [Test]
    public void PlayerEntity_ShouldInitializeCorrectly()
    {
        // Arrange 
        const string id = "2";
        const string username = "test";
        const string password = "test";
        const int groupId = 3;
        const string color = "blue";
        var from = DateTime.Now;
        var to = DateTime.Now.AddDays(1);
        var freeTime = new FreeTime { From = from, To = to };

        //Act
        var playerEntity = new PlayerEntity { Id = id, Username = username, Password = password, GroupId = groupId, Color = color, FreeTime = freeTime };

        // Assert
        playerEntity.Should().NotBeNull();
        playerEntity.Id.Should<string>().NotBe(string.Empty);
        playerEntity.Username.Should<string>().NotBe(string.Empty);
        playerEntity.Password.Should<string>().NotBe(string.Empty);
        playerEntity.GroupId.Should<int>().NotBe(default);
        playerEntity.Color.Should<string>().NotBe(string.Empty);
        playerEntity.FreeTime.Should().NotBeNull().Should().NotBe(default);

        playerEntity.Id.Should<string>().Be(id);
        playerEntity.Username.Should<string>().Be(username);
        playerEntity.Password.Should<string>().Be(password);
        playerEntity.GroupId.Should<int>().Be(groupId);
        playerEntity.Color.Should<string>().Be(color);
        playerEntity.FreeTime.Should().Be(freeTime);
    }
}