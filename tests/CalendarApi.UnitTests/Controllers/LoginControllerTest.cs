using App.Metrics;
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
using Newtonsoft.Json.Linq;
using ConfigurationProvider = CalendarApi.UnitTests.Helpers.ConfigurationProvider;

namespace CalendarApi.UnitTests.Controllers;

public class LoginControllerTest
{
    private LoginController _sut;
    private Mock<IMongoRepository> _mongoRepository;
    
    [SetUp]
    public void Setup()
    {
        _mongoRepository = new Mock<IMongoRepository>(MockBehavior.Strict);
        _sut = new LoginController(_mongoRepository.Object, ConfigurationProvider.Configuration);
        new MetricsBuilder().Build();
    }
    
    [TearDown]
    public void TearDown() => _mongoRepository.VerifyNoOtherCalls();
    
    [Test]
    public async Task Login_WhenRequestIsValid_ShouldReturnOk()
    {
        // Arrange
        var request = new LoginRequest { Username = "username", Password = "password" };
        var playerResponse = new PlayerResponse();
        _mongoRepository
            .Setup(x => x.GetPlayer(It.IsAny<PlayerEntity>()))
            .ReturnsAsync(Result.Success<PlayerResponse, Exception>(playerResponse));
        
        // Act
        var result = await _sut.Login(request) as OkObjectResult;
        var value = (LoginResponse)result!.Value!;
        
        // Assert
        result.Should().NotBeNull();
        result.Value.Should().NotBeNull();
        result.StatusCode.Should<int>().Be(StatusCodes.Status200OK);
        value.Should().NotBeNull();
        value.Player.Should().BeEquivalentTo(playerResponse);
        
        _mongoRepository.Verify(x => x.GetPlayer(It.IsAny<PlayerEntity>()), Times.Once);
    }
    
    [Test]
    public async Task Login_WhenRequestUsernameIsEmpty_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new LoginRequest { Username = "", Password = "password" };
        var playerResponse = new PlayerResponse();
        _mongoRepository
            .Setup(x => x.GetPlayer(It.IsAny<PlayerEntity>()))
            .ReturnsAsync(Result.Success<PlayerResponse, Exception>(playerResponse));
        
        // Act
        var result = await _sut.Login(request) as BadRequestObjectResult;
        
        // Assert
        result.Should().NotBeNull();
        result.Value.Should().NotBeNull();
        result.StatusCode.Should<int>().Be(StatusCodes.Status400BadRequest);
        result.Value.Should().BeEquivalentTo("Username should not be empty or null\r\n");
        
        _mongoRepository.Verify(x => x.GetPlayer(It.IsAny<PlayerEntity>()), Times.Never);
    }
    
    [Test]
    public async Task Login_WhenRequestPasswordIsEmpty_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new LoginRequest { Username = "username", Password = "" };
        var playerResponse = new PlayerResponse();
        _mongoRepository
            .Setup(x => x.GetPlayer(It.IsAny<PlayerEntity>()))
            .ReturnsAsync(Result.Success<PlayerResponse, Exception>(playerResponse));
        
        // Act
        var result = await _sut.Login(request) as BadRequestObjectResult;
        
        // Assert
        result.Should().NotBeNull();
        result.Value.Should().NotBeNull();
        result.StatusCode.Should<int>().Be(StatusCodes.Status400BadRequest);
        result.Value.Should().BeEquivalentTo("Password should be at least 8 characters long\r\n");
        
        _mongoRepository.Verify(x => x.GetPlayer(It.IsAny<PlayerEntity>()), Times.Never);
    }
    
    [Test]
    public async Task Login_WhenDatabaseReturnsException_ShouldReturnInternalServerError()
    {
        // Arrange
        var request = new LoginRequest { Username = "username", Password = "password" };
        _mongoRepository
            .Setup(x => x.GetPlayer(It.IsAny<PlayerEntity>()))
            .ReturnsAsync(Result.Failure<PlayerResponse, Exception>(new ()));
        
        // Act
        var result = await _sut.Login(request) as StatusCodeResult;
        
        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should<int>().Be(StatusCodes.Status500InternalServerError);
        
        _mongoRepository.Verify(x => x.GetPlayer(It.IsAny<PlayerEntity>()), Times.Once);
    }
    
    [Test]
    public async Task Register_WhenRequestIsValid_ShouldReturnOk()
    {
        // Arrange
        var request = new RegisterRequest { Username = "username", Password = "password", Color = "color" };
        var playerResponse = new PlayerResponse();
        _mongoRepository
            .Setup(x => x.CheckIfUserExists(It.IsAny<PlayerEntity>()))
            .ReturnsAsync(Result.Success<PlayerEntity, Exception>(new ()));
        
        _mongoRepository
            .Setup(x => x.RegisterPlayer(It.IsAny<PlayerEntity>()))
            .ReturnsAsync(Result.Success<PlayerResponse, Exception>(playerResponse));
        
        // Act
        var result = await _sut.Register(request) as OkObjectResult;
        var value = (LoginResponse)result!.Value!;
        
        // Assert
        result.Should().NotBeNull();
        result.Value.Should().NotBeNull();
        result.StatusCode.Should<int>().Be(StatusCodes.Status200OK);
        value.Should().NotBeNull();
        value.Player.Should().BeEquivalentTo(playerResponse);
        
        _mongoRepository.Verify(x => x.CheckIfUserExists(It.IsAny<PlayerEntity>()), Times.Once);
        _mongoRepository.Verify(x => x.RegisterPlayer(It.IsAny<PlayerEntity>()), Times.Once);
    }
    
    [Test]
    public async Task Register_WhenRequestIsInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new RegisterRequest { Username = "", Password = "password", Color = "color" };
        var playerResponse = new PlayerResponse();
        _mongoRepository
            .Setup(x => x.CheckIfUserExists(It.IsAny<PlayerEntity>()))
            .ReturnsAsync(Result.Success<PlayerEntity, Exception>(new ()));
        
        _mongoRepository
            .Setup(x => x.RegisterPlayer(It.IsAny<PlayerEntity>()))
            .ReturnsAsync(Result.Success<PlayerResponse, Exception>(playerResponse));
        
        // Act
        var result = await _sut.Register(request) as BadRequestObjectResult;
        
        // Assert
        result.Should().NotBeNull();
        result.Value.Should().NotBeNull();
        result.StatusCode.Should<int>().Be(StatusCodes.Status400BadRequest);
        result.Value.Should().BeEquivalentTo("Username should not be empty or null\r\n");
        
        _mongoRepository.Verify(x => x.CheckIfUserExists(It.IsAny<PlayerEntity>()), Times.Never);
        _mongoRepository.Verify(x => x.RegisterPlayer(It.IsAny<PlayerEntity>()), Times.Never);
    }
    
    [Test]
    public async Task Register_WhenRegisterPlayerReturnsException_ShouldReturnInternalServerError()
    {
        // Arrange
        var request = new RegisterRequest { Username = "username", Password = "password", Color = "color" };
        _mongoRepository
            .Setup(x => x.CheckIfUserExists(It.IsAny<PlayerEntity>()))
            .ReturnsAsync(Result.Success<PlayerEntity, Exception>(new ()));
        
        _mongoRepository
            .Setup(x => x.RegisterPlayer(It.IsAny<PlayerEntity>()))
            .ReturnsAsync(Result.Failure<PlayerResponse, Exception>(new ()));
        
        // Act
        var result = await _sut.Register(request) as StatusCodeResult;
        
        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should<int>().Be(StatusCodes.Status500InternalServerError);
        
        _mongoRepository.Verify(x => x.CheckIfUserExists(It.IsAny<PlayerEntity>()), Times.Once);
        _mongoRepository.Verify(x => x.RegisterPlayer(It.IsAny<PlayerEntity>()), Times.Once);
    }
    
    [Test]
    public async Task Register_WhenCheckIfUserExistsReturnsException_ShouldReturnInternalServerError()
    {
        // Arrange
        var request = new RegisterRequest { Username = "username", Password = "password", Color = "color" };
        _mongoRepository
            .Setup(x => x.CheckIfUserExists(It.IsAny<PlayerEntity>()))
            .ReturnsAsync(Result.Failure<PlayerEntity, Exception>(new ()));
        
        _mongoRepository
            .Setup(x => x.RegisterPlayer(It.IsAny<PlayerEntity>()))
            .ReturnsAsync(Result.Success<PlayerResponse, Exception>(new ()));
        
        // Act
        var result = await _sut.Register(request) as StatusCodeResult;
        
        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should<int>().Be(StatusCodes.Status500InternalServerError);
        
        _mongoRepository.Verify(x => x.CheckIfUserExists(It.IsAny<PlayerEntity>()), Times.Once);
        _mongoRepository.Verify(x => x.RegisterPlayer(It.IsAny<PlayerEntity>()), Times.Never);
    }
}