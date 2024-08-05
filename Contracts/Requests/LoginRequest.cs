using MongoDB.Bson.Serialization.Attributes;

public sealed class LoginRequest
{
    [BsonId]
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
