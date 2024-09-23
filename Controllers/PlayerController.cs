using CalendarApi.Contracts.Requests;
using CalendarApi.Internal;
using CalendarApi.Repository;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CalendarApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class PlayerController([FromServices] IMongoRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GroupIdRequest request) =>
        await request
            .Validate()
            .Tap(x => Log.Debug("Getting all players for GroupId: {Id}", x.GroupId))
            .Bind(x => repository.GetAll(x.GroupId))
            .LogError()
            .Match(Ok, e => e.ToActonResult());

    [HttpPost("update-free-time")]
    public async Task<IActionResult> UpdateFreeTime([FromBody] UpdateFreeTimeRequest request) =>
        await request
            .Validate()
            .Tap(x => Log.Debug("Updating free time for PlayerId: {Id}", x.PlayerId))
            .Map(x => x.ToEntity())
            .Bind(repository.UpdateFreeTime)
            .LogError()
            .Match(x => Ok(x), e => e.ToActonResult());
}
