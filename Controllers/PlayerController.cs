using CalendarApi.Contracts.Requests;
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
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GroupIdRequest request) =>
        await request.Validate()
            .Bind(x => repository.GetAll(x.GroupId))
            .Match(Ok, e => e.ToActonResult());

    [HttpPost("update-free-time")]
    public async Task<IActionResult> UpdateFreeTime([FromBody] UpdateFreeTimeRequest request) =>
        await request.Validate()
            .Map(x => x.ToEntity())
            .Bind(repository.UpdateFreeTime)
            .Match(x => Ok(x), e => e.ToActonResult());
}