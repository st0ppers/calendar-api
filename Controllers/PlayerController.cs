using CalendarApi.Internal;
using CalendarApi.Repository;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalendarApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class PlayerController([FromServices] IMongoRepository repository) : ControllerBase
{
    [HttpGet("{groupId:int}")]
    public async Task<IActionResult> GetAll(int groupId) =>
        await repository.GetAll(groupId)
            .Match(Ok, e => e.ToActonResult());
}