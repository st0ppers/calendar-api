using CalendarApi.Contracts.Requests;
using FluentAssertions;
using ProtoBuf;

namespace CalendarApi.UnitTests.Contracts.Requests;

public class GroupIdRequestTest
{
    
    [Test]
    public void GroupIdRequest_ShouldInitializeCorrectly()
    {
        // Arrange 
        const int groupId = 1;
        
        //Act
        var groupIdRequest = new GroupIdRequest { GroupId = groupId };
        
        // Assert
        groupIdRequest.Should().NotBeNull();
        groupIdRequest.GroupId.Should<int>().NotBe(default);
        
        groupIdRequest.GroupId.Should<int>().Be(groupId);
    }
}