using CalendarApi.Contracts.Entities;
using CalendarApi.Contracts.Requests;
using CalendarApi.Contracts.Response;
using CalendarApi.Controllers;
using CalendarApi.Repository;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CalendarApi.UnitTests.Controllers;

public class PlayerControllerTest
{
    private PlayerController _sut;
    private Mock<IMongoRepository> _mongoRepository;

    [SetUp]
    public void Setup()
    {
        _mongoRepository = new Mock<IMongoRepository>(MockBehavior.Strict);
        _sut = new PlayerController(_mongoRepository.Object);
    }

    [TearDown]
    public void TearDown() => _mongoRepository.VerifyNoOtherCalls();

    [Test]
    public async Task GetAll_WhenRequestIsValid_ShouldReturnOk()
    {
        // Arrange
        var groupId = 6;
        var request = new GroupIdRequest { GroupId = groupId };
        var playerResponse = new PlayerResponse();
        _mongoRepository
            .Setup(x => x.GetAll(It.IsAny<int>()))
            .ReturnsAsync(Result.Success<IEnumerable<PlayerResponse>, Exception>([playerResponse]));

        // Act
        var result = await _sut.GetAll(request) as OkObjectResult;
        var value = (IEnumerable<PlayerResponse>)result!.Value!;

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().NotBeNull();
        result.StatusCode.Should<int>().Be(StatusCodes.Status200OK);
        value.Should().NotBeEmpty();
        value.Should().Contain(playerResponse);

        _mongoRepository.Verify(x => x.GetAll(groupId), Times.Once);
    }

    [Test]
    public async Task GetAll_WhenDatabaseReturnsException_ShouldReturnBadRequest()
    {
        // Arrange
        var groupId = 1;
        var request = new GroupIdRequest { GroupId = groupId };
        _mongoRepository
            .Setup(x => x.GetAll(It.IsAny<int>()))
            .ReturnsAsync(Result.Failure<IEnumerable<PlayerResponse>, Exception>(new()));

        // Act
        var result = await _sut.GetAll(request) as StatusCodeResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should<int>().Be(StatusCodes.Status500InternalServerError);

        _mongoRepository.Verify(x => x.GetAll(groupId), Times.Once);
    }


    [Test]
    public async Task GetAll_WhenRequestIsInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        var groupId = -1;
        var request = new GroupIdRequest { GroupId = groupId };
        var playerResponse = new PlayerResponse();
        _mongoRepository
            .Setup(x => x.GetAll(It.IsAny<int>()))
            .ReturnsAsync(Result.Success<IEnumerable<PlayerResponse>, Exception>([playerResponse]));

        // Act
        var result = await _sut.GetAll(request) as BadRequestObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should<int>().Be(StatusCodes.Status400BadRequest);
        result.Value.Should().BeEquivalentTo("GroupId should be greater than 0\r\n");

        _mongoRepository.Verify(x => x.GetAll(groupId), Times.Never);
    }

    [Test]
    public async Task UpdateFreeTime_WhenRequestIsValid_ShouldReturnOk()
    {
        // Arrange
        const long response = 1L;
        var request = new UpdateFreeTimeRequest();
        _mongoRepository
            .Setup(x => x.UpdateFreeTime(It.IsAny<UpdateFreeTimeEntity>()))
            .ReturnsAsync(Result.Success<long, Exception>(response));

        // Act
        var result = await _sut.UpdateFreeTime(request) as OkObjectResult;
        var value = (long)result!.Value!;

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().NotBeNull();
        result.StatusCode.Should<int>().Be(StatusCodes.Status200OK);
        value.Should().Be(response);

        _mongoRepository.Verify(x => x.UpdateFreeTime(It.IsAny<UpdateFreeTimeEntity>()), Times.Once);
    }

    [Test]
    public async Task UpdateFreeTime_WhenDatabaseReturnsException_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new UpdateFreeTimeRequest { From = DateTime.Now.AddDays(-1), To = DateTime.Now };
        _mongoRepository
            .Setup(x => x.UpdateFreeTime(It.IsAny<UpdateFreeTimeEntity>()))
            .ReturnsAsync(Result.Failure<long, Exception>(new()));

        // Act
        var result = await _sut.UpdateFreeTime(request) as StatusCodeResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should<int>().Be(StatusCodes.Status500InternalServerError);

        _mongoRepository.Verify(x => x.UpdateFreeTime(It.IsAny<UpdateFreeTimeEntity>()), Times.Once);
    }

    [Test]
    public async Task UpdateFreeTime_WhenRequestIsInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        const long response = 1L;
        var request = new UpdateFreeTimeRequest { From = DateTime.Now, To = DateTime.Now.AddDays(-1) };
        _mongoRepository
            .Setup(x => x.UpdateFreeTime(It.IsAny<UpdateFreeTimeEntity>()))
            .ReturnsAsync(Result.Success<long, Exception>(response));

        // Act
        var result = await _sut.UpdateFreeTime(request) as BadRequestObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().NotBeNull();
        result.StatusCode.Should<int>().Be(StatusCodes.Status400BadRequest);
        result.Value.Should().BeEquivalentTo("From should be less than To\r\n");

        _mongoRepository.Verify(x => x.UpdateFreeTime(It.IsAny<UpdateFreeTimeEntity>()), Times.Never);
    }
}