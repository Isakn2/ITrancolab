// Hubs/PresentationHub.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using CollaborativePresentations.Models;
using CollaborativePresentations.Data;

namespace CollaborativePresentations.Hubs;

public class PresentationHub : Hub
{
    private readonly ApplicationDbContext _context;

    public PresentationHub(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task JoinPresentation(string presentationId, string nickname)
    {
        try 
        {
            // Validate inputs
            if (string.IsNullOrEmpty(presentationId) || string.IsNullOrEmpty(nickname))
            {
                throw new HubException("Presentation ID and nickname are required");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, presentationId);
            
            var userConnection = new UserConnection
            {
                ConnectionId = Context.ConnectionId,
                PresentationId = presentationId,
                Nickname = nickname,
                IsEditor = false
            };
            
            _context.UserConnections.Add(userConnection);
            await _context.SaveChangesAsync();
            
            await Clients.Group(presentationId).SendAsync("UserJoined", nickname);
            await SendUserList(presentationId);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", $"Failed to join: {ex.Message}");
            throw; // Re-throw for logging
        }
    }

    public async Task MakeEditor(string presentationId, string connectionId, bool isEditor)
    {
        var user = await _context.UserConnections.FindAsync(connectionId);
        if (user != null)
        {
            user.IsEditor = isEditor;
            await _context.SaveChangesAsync();
            await SendUserList(presentationId);
        }
    }

    public async Task UpdateTextBlock(string presentationId, TextBlock textBlock)
    {
        try
        {
            // Verify the user has permission to edit
            var user = await _context.UserConnections
                .FirstOrDefaultAsync(u => u.ConnectionId == Context.ConnectionId);
                
            if (user == null || !user.IsEditor)
            {
                throw new HubException("You don't have editing permissions");
            }

            var existingBlock = await _context.TextBlocks.FindAsync(textBlock.Id);
            if (existingBlock != null)
            {
                existingBlock.Content = textBlock.Content;
                existingBlock.Top = textBlock.Top;
                existingBlock.Left = textBlock.Left;
                existingBlock.Width = textBlock.Width;
                existingBlock.Height = textBlock.Height;
            }
            else
            {
                _context.TextBlocks.Add(textBlock);
            }
            
            await _context.SaveChangesAsync();
            await Clients.GroupExcept(presentationId, Context.ConnectionId)
                .SendAsync("TextBlockUpdated", textBlock);
        }
        catch (DbUpdateException)
        {
            await Clients.Caller.SendAsync("Error", "Failed to save changes");
        }
    }

    private async Task SendUserList(string presentationId)
    {
        var users = await _context.UserConnections
            .AsNoTracking() // Important for read-only operations
            .Where(u => u.PresentationId == presentationId)
            .Select(u => new { 
                u.ConnectionId, 
                u.Nickname, 
                u.IsEditor 
            })
            .ToListAsync();
            
        await Clients.Group(presentationId).SendAsync("UpdateUserList", users);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        try 
        {
            var user = await _context.UserConnections
                .FirstOrDefaultAsync(u => u.ConnectionId == Context.ConnectionId);
                
            if (user != null)
            {
                _context.UserConnections.Remove(user);
                await _context.SaveChangesAsync();
                
                if (!string.IsNullOrEmpty(user.PresentationId))
                {
                    await SendUserList(user.PresentationId);
                    await Clients.Group(user.PresentationId)
                        .SendAsync("UserLeft", user.Nickname);
                }
            }
        }
        finally 
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}