using MongoDB.Bson.Serialization.Attributes;

namespace CalendarApi.Contracts;

public sealed class FreeTime
{
    [BsonElement("from")] public DateTime From { get; set; }
    [BsonElement("to")] public DateTime To { get; set; }
}