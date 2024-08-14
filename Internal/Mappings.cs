using CalendarApi.Contracts.Entities;
using CalendarApi.Contracts.Response;

namespace CalendarApi.Internal;

public static class Mappings
{
    public static PlayerEntity ToPlayerEntity(this LoginRequest request) => new() { Username = request.Username, Password = request.Password };
    public static LoginEntity ToLoginEntity(this LoginRequest request) => new() { Username = request.Username, Password = request.Password };
    public static PlayerResponse ToResponse(this PlayerEntity entity) => new() { Username = entity.Username, Password = entity.Password };
}