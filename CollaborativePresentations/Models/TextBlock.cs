// Models/TextBlock.cs
public class TextBlock
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? SlideId { get; set; }
    public Slide? Slide { get; set; }
    public string Content { get; set; } = string.Empty; // Default value
    public double Top { get; set; }
    public double Left { get; set; }
    public double Width { get; set; } = 300;
    public double Height { get; set; } = 200;
}