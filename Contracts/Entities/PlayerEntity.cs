using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CalendarApi.Contracts.Entities;

public sealed class PlayerEntity
{
    [BsonId] public ObjectId Id { get; init; }
    [BsonElement("username")] public string Username { get; init; } = string.Empty;
    [BsonElement("password")] public string Password { get; init; } = string.Empty;
    [BsonElement("groupId")] public int GroupId { get; init; } = 1;
    [BsonElement("color")] public string Color { get; init; } = string.Empty;
    [BsonElement("freeTime")] public FreeTime FreeTime { get; init; } = new();
}