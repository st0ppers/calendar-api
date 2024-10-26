using CalendarApi.Contracts.Entities;
using CalendarApi.Contracts.Requests;
using CalendarApi.Contracts.Response;

namespace CalendarApi.Internal;

public static class Mappings
{
    public static PlayerEntity RequestToEntity(RegisterRequest request) => new() { Username = request.Username, Password = request.Password, Color = request.Color };
    public static PlayerEntity RequestToPlayerEntity(LoginRequest request) => new() { Username = request.Username, Password = request.Password };
    public static PlayerResponse ToResponse(PlayerEntity entity) => new() { Username = entity.Username, Color = entity.Color, FreeTime = entity.FreeTime, Id = entity.Id };
    public static UpdateFreeTimeEntity ToEntity(UpdateFreeTimeRequest request) => new() { PlayerId = request.PlayerId, From = request.From, To = request.To };
}