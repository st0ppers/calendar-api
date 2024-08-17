using CalendarApi.Contracts.Entities;
using CalendarApi.Contracts.Requests;
using CalendarApi.Contracts.Response;

namespace CalendarApi.Internal;

public static class Mappings
{
    public static PlayerEntity RegisterToLogin(this RegisterRequest request) => new() { Username = request.Username, Password = request.Password, Color = request.Color };
    public static PlayerEntity LoginToPlayerEntity(this LoginRequest request) => new() { Username = request.Username, Password = request.Password };
    public static PlayerResponse ToResponse(this PlayerEntity entity) => new() { Username = entity.Username, Color = entity.Color, FreeTime = entity.FreeTime };
}