
public sealed class PlayerResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public FreeTime FreeTime { get; set; } = new();
}
