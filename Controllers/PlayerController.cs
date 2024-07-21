using Microsoft.AspNetCore.Mvc;

namespace CalendarApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class PlayerController([FromServices] IPlayerRepository repository) : ControllerBase
{

    [HttpGet("{id:int}")]
    public PlayerResponse Get([FromRoute] string id)
    {
        Console.WriteLine(id);
        return new PlayerResponse()
        {
            Color = "Blue",
            Name = "Alex",
            FreeTime = new() { From = DateTime.UtcNow.AddDays(-1), To = DateTime.UtcNow }
        };
    }

    [HttpGet("{id:int}/player")]
    public PlayerResponse Get([FromRoute] int id)
    {
        Console.WriteLine(id);
        return new PlayerResponse()
        {
            Color = "Blue",
            Name = "Alex",
            FreeTime = new() { From = DateTime.UtcNow.AddDays(-1), To = DateTime.UtcNow }
        };
    }

    // await repository.Get(id)
    // .Match(Ok, Problem);


    [HttpGet("all")]
    public IEnumerable<PlayerResponse> GetAll()
    {
        return new PlayerResponse[]
        {

      new PlayerResponse(){
        Color= "Blue",
        Name= "Alex",
        FreeTime= new () {From= DateTime.UtcNow.AddDays(-1), To= DateTime.UtcNow}
      },

      new PlayerResponse(){
        Color= "Pink",
        Name= "Petur",
        FreeTime= new () {From= DateTime.UtcNow.AddDays(-1), To= DateTime.UtcNow}
      },

      new PlayerResponse(){
        Color= "Red",
        Name= "Nasko",
        FreeTime= new () {From= DateTime.UtcNow.AddDays(-1), To= DateTime.UtcNow}
      },

      new PlayerResponse(){
        Color= "Green",
        Name= "Jak",
        FreeTime= new () {From= DateTime.UtcNow.AddDays(-1), To= DateTime.UtcNow}
      },
    };

    }
}

// public class Api : ControllerBase
// {
//     //TODO: Add different types of exceptions
//     //TODO: Move to different file
//     public IActionResult Problem() =>
//         e switch
//         {
//             NullReferenceException nre => Problem(title: "NotFound", statusCode: 404, detail: nre.StackTrace),
//             _ => Problem(title: "InternalServerError", statusCode: 500, detail: e.StackTrace)
//         };
// }
