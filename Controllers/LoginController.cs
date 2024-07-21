using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace CalendarApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class LoginController : ControllerBase
{

    [HttpGet]
    public IEnumerable<int> Get()
    {
        return new int[] { 1, 2, 3, 4, 5 };
    }

    [HttpPost]
    public ActionResult<Result<bool, Exception>> Login(LoginRequest request)
    {
        if (request.Username == "a" && request.Password == "a")
        {
            return Ok(true);
        }
        return Ok(false);
    }
}
