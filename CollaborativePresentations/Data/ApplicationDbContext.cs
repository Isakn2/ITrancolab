// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using CollaborativePresentations.Models;

namespace CollaborativePresentations.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Presentation> Presentations { get; set; }
    public DbSet<Slide> Slides { get; set; }
    public DbSet<TextBlock> TextBlocks { get; set; }
    public DbSet<UserConnection> UserConnections { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure Presentation entity
        modelBuilder.Entity<Presentation>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasMany(p => p.Slides)
                  .WithOne(s => s.Presentation)
                  .HasForeignKey(s => s.PresentationId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Slide entity
        modelBuilder.Entity<Slide>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.HasMany(s => s.TextBlocks)
                  .WithOne(t => t.Slide)
                  .HasForeignKey(t => t.SlideId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure TextBlock entity
        modelBuilder.Entity<TextBlock>(entity =>
        {
            entity.HasKey(t => t.Id);
        });

        // Configure UserConnection entity
        modelBuilder.Entity<UserConnection>(entity =>
        {
            entity.HasKey(u => u.ConnectionId); // Using ConnectionId as primary key
            entity.HasOne(u => u.Presentation)
                  .WithMany(p => p.ConnectedUsers)
                  .HasForeignKey(u => u.PresentationId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}