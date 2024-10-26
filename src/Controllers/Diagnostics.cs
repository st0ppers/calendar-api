using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace CalendarApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiagnosticsController(IConfiguration config)
{
    [HttpGet]
    public string GetConfigs()
    {
        var t = config.AsEnumerable().ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        return JsonSerializer.Serialize(t);
    }
}