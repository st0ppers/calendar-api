using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CalendarApi.Contracts.Entities;

public sealed class PlayerEntity
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("username")]
    public string Username { get; set; } = string.Empty;

    [BsonElement("password")]
    public string Password { get; set; } = string.Empty;
}
