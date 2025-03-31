// Models/Presentation.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CollaborativePresentations.Data;
using CollaborativePresentations.Hubs;

public class Presentation
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    public string Title { get; set; } = "Untitled";
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CreatorNickname { get; set; } = "Anonymous";
    
    public List<Slide> Slides { get; set; } = new List<Slide>();
    public List<UserConnection> ConnectedUsers { get; set; } = new List<UserConnection>();
}