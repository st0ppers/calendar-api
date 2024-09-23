using CalendarApi.Contracts.Configurations;
using CalendarApi.Contracts.Requests;
using CalendarApi.Contracts.Response;
using CalendarApi.Internal;
using CalendarApi.Repository;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using static CalendarApi.Internal.Mappings;

namespace CalendarApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController(IMongoRepository repo, IConfiguration config) : ControllerBase
{
    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request) =>
        await request
            .Validate()
            .Tap(x => Log.Debug("Logging in {Username}", x.Username))
            .Map(RequestToPlayerEntity)
            .Bind(repo.GetPlayer)
            .Map(PlayerToLoginResponse)
            .CountRequest()
            .CountFailedRequest()
            .LogError()
            .Match(Ok, e => e.ToActonResult());

    [Route("register")]
    [HttpPost]
    public async Task<ActionResult> Register(RegisterRequest request) =>
        await request
            .Validate()
            .Tap(x => Log.Debug("Registering {Username}", x.Username))
            .Map(RequestToEntity)
            .Bind(repo.CheckIfUserExists)
            .Bind(repo.RegisterPlayer)
            .Map(PlayerToLoginResponse)
            .CountRequest()
            .CountFailedRequest()
            .LogError()
            .Match(Ok, e => e.ToActonResult());

    private LoginResponse PlayerToLoginResponse(PlayerResponse response)
    {
        var jwt = config.GetRequiredSection(JwtOptions.Section).Get<JwtOptions>();
        //TODO Add test when this is null
        var token = TokenEndpoint.Connect(jwt!, response);
        return new()
        {
            Token = token.AccessToken,
            Player = response,
            Expiration = token.Expiration,
        };
    }
}