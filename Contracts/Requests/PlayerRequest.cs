public sealed class PlayerRequest
{
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public FreeTime FreeTime { get; set; } = new();
}
