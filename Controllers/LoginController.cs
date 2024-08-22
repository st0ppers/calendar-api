using CalendarApi.Contracts.Requests;
using CalendarApi.Contracts.Response;
using CalendarApi.Internal;
using CalendarApi.Repository;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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
            .Map(x => x.LoginToPlayerEntity())
            .Bind(repo.GetPlayer)
            .Map(player =>
            {
                //TODO Add test when this is null
                var token = GetToken(player);
                return new LoginResponse
                {
                    Token = token.AccessToken,
                    Player = player,
                    Expiration = token.Expiration,
                };
            })
            .TapError(e => Log.Error("Error in login with message: {Error}", e.Message))
            .Match(Ok, e => e.ToActonResult());

    [Route("register")]
    [HttpPost]
    public async Task<ActionResult> Register(RegisterRequest request) =>
        await request
            .Validate()
            .Tap(x => Log.Debug("Registering {Username}", x.Username))
            .Map(x => x.RegisterToLogin())
            .Bind(repo.CheckIfUserExists)
            .Bind(repo.RegisterPlayer)
            .Map(player =>
            {
                //TODO Add test when this is null
                var token = GetToken(player);
                return new LoginResponse
                {
                    Token = token.AccessToken,
                    Player = player,
                    Expiration = token.Expiration,
                };
            })
            .TapError(e => Log.Error("Error in register with message: {Error}", e.Message))
            .Match(Ok, e => e.ToActonResult());

    private TokenResponse GetToken(PlayerResponse player)
    {
        var jwt = config.GetRequiredSection(JwtOptions.Section).Get<JwtOptions>();
        return TokenEndpoint.Connect(jwt!, player);
    }
}
