using CalendarApi.Internal;
using Microsoft.AspNetCore.Mvc;

namespace CalendarApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TokenController(IConfiguration config) : ControllerBase
{
    [HttpPost]
    public IActionResult GetToken()
    {
        var jwt = config.GetRequiredSection(JwtOptions.Section).Get<JwtOptions>();
        return Ok(TokenEndpoint.Connect(jwt!));
    }
}
