using Microsoft.AspNetCore.Mvc;

namespace CalendarApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    [HttpGet]
    public IEnumerable<int> Get()
    {
        return new int[] { 1, 2, 3, 4, 5 };
    }
}
