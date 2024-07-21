using Microsoft.AspNetCore.Mvc;

namespace CalendarApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CalendarController : ControllerBase
{

    [HttpGet]
    public IEnumerable<int> Get()
    {
        return new int[] { 1, 2, 3, 4, 5 };
    }
}
