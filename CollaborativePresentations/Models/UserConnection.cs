// Models/UserConnection.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserConnection
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } 
    
    [Required]
    public string? PresentationId { get; set; }
    public Presentation? Presentation { get; set; }
   
    [Required, MaxLength(100)]
    public string? Nickname { get; set; }

    [Required, MaxLength(50)]
    public string? ConnectionId { get; set; }
    
    public bool IsEditor { get; set; } = false;
    public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;
}