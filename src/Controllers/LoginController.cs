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
            .Bind(PlayerToLoginResponse)
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
            .Bind(PlayerToLoginResponse)
            .CountRequest()
            .CountFailedRequest()
            .LogError()
            .Match(Ok, e => e.ToActonResult());

    private Result<LoginResponse, Exception> PlayerToLoginResponse(PlayerResponse response)
    {
        var jwt = config.GetRequiredSection(JwtOptions.Section).Get<JwtOptions>();
        return TokenEndpoint.Connect(jwt!, response)
            .Map(t => new LoginResponse() { Token = t.AccessToken, Player = response, Expiration = t.Expiration });
    }
}