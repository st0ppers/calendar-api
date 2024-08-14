using CalendarApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalendarApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class PlayerController([FromServices] IMongoRepository repository) : ControllerBase
{
    // [HttpGet("{id:int}")]
    // public PlayerEntity Get([FromRoute] string id)
    // {
    //     var a = repository;
    //     return new()
    //     {
    //         Color = "Blue",
    //         Name = "Alex",
    //         FreeTime = new() { From = DateTime.UtcNow.AddDays(-1), To = DateTime.UtcNow }
    //     };
    // }
    //
    // [HttpGet("{id:int}/player")]
    // public PlayerResponse Get([FromRoute] int id)
    // {
    //     return new()
    //     {
    //         Color = "Blue",
    //         Name = "Alex",
    //         FreeTime = new() { From = DateTime.UtcNow.AddDays(-1), To = DateTime.UtcNow }
    //     };
    // }
    //
    // [HttpGet("all")]
    // public IEnumerable<PlayerResponse> GetAll()
    // {
    //     return new PlayerResponse[]
    //     {
    //         new()
    //         {
    //             Color = "Blue",
    //             Name = "Alex",
    //             FreeTime = new() { From = DateTime.UtcNow.AddDays(-1), To = DateTime.UtcNow }
    //         },
    //         new()
    //         {
    //             Color = "Pink",
    //             Name = "Petur",
    //             FreeTime = new() { From = DateTime.UtcNow.AddDays(-1), To = DateTime.UtcNow }
    //         },
    //         new()
    //         {
    //             Color = "Red",
    //             Name = "Nasko",
    //             FreeTime = new() { From = DateTime.UtcNow.AddDays(-1), To = DateTime.UtcNow }
    //         },
    //         new()
    //         {
    //             Color = "Green",
    //             Name = "Jak",
    //             FreeTime = new() { From = DateTime.UtcNow.AddDays(-1), To = DateTime.UtcNow }
    //         },
    //     };
    // }
}