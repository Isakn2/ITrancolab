// Models/Slide.cs
public class Slide
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public int Order { get; set; }
    public string? PresentationId { get; set; }
    public Presentation? Presentation { get; set; }
    public List<TextBlock> TextBlocks { get; set; } = new List<TextBlock>();
}