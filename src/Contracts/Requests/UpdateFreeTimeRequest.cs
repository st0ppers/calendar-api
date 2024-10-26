using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CalendarApi.Contracts.Requests;

public sealed class UpdateFreeTimeRequest
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string PlayerId { get; init; }
    public DateTime From { get; init; }
    public DateTime To { get; init; }
}