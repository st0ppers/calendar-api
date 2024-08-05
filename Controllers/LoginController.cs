using System.Text.Json;
using CalendarApi.Internal;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace CalendarApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController(IMongoRepository repo, IConfiguration config) : ControllerBase
{
    // [Route("login")]
    // [HttpPost]
    // public async Task<bool> Login(LoginRequest request)
    // {
    //     var player = await repo.GetPlayer(request);
    //     Console.WriteLine(JsonSerializer.Serialize(player.Value));
    //     return player.Match(player => true, e => false);
    // }

    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var player = await repo.GetPlayer(request)
            .Map(p =>
            {
                Console.WriteLine(JsonSerializer.Serialize(p));
                if (p.Username == request.Username && p.Password == request.Password)
                {
                    return GetToken();
                }
                return BadRequest("Invalid username or password");
            });
        return player.Match(token => token, e => BadRequest(e.Message)); //TODO Return player with access token, not just token
    }

    [Route("register")]
    [HttpPost]
    public async Task<string> Register(LoginRequest request)
    {
        var player = await repo.RegisterPlayer(request);
        return player.Match(player => $"{player}", e => e.Message);
    }

    [Route("reset-password")]
    [HttpPost]
    public async Task<string> ResetPassword(LoginRequest request)
    {
        var player = await repo.GetPlayer(request);
        return player.Match(player => $"Welcome {player.Username}", e => e.Message);
    }

    private IActionResult GetToken()
    {
        var jwt = config.GetRequiredSection(JwtOptions.Section).Get<JwtOptions>();
        return Ok(TokenEndpoint.Connect(jwt!));
    }
}
