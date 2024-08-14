using CalendarApi.Contracts.Response;
using CalendarApi.Internal;
using CalendarApi.Repository;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace CalendarApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController(IMongoRepository repo, IConfiguration config) : ControllerBase
{
    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        return await request.Validate()
            .Map(x => x.ToLoginEntity())
            .Bind(repo.GetPlayer)
            .Map(player =>
            {
                //TODO Add test when this is null
                var token = GetToken(player);
                return new LoginResponse { Token = token.AccessToken, Player = player, Expiration = token.Expiration };
            })
            .Match(Ok, e => e.ToActonResult());
    }

    [Route("register")]
    [HttpPost]
    public async Task<ActionResult> Register(LoginRequest request)
    {
        return await request.Validate()
            .Map(x => x.ToLoginEntity())
            .Bind(repo.CheckIfUserExists)
            .Map(_ => request.ToPlayerEntity())
            .Bind(repo.RegisterPlayer)
            .Match(Ok, e => e.ToActonResult());
    }

    private TokenResponse GetToken(PlayerResponse player)
    {
        var jwt = config.GetRequiredSection(JwtOptions.Section).Get<JwtOptions>();
        return TokenEndpoint.Connect(jwt, player);
    }
}