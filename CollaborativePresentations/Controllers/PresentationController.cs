// Controllers/PresentationController.cs
using CollaborativePresentations.Data;
using Microsoft.AspNetCore.Mvc;
using CollaborativePresentations.Models;
using Microsoft.EntityFrameworkCore;

public class PresentationController : Controller
{
    private readonly ApplicationDbContext _context;

    public PresentationController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Edit(string id, string nickname)
    {
        var presentation = _context.Presentations
            .Include(p => p.Slides)
            .FirstOrDefault(p => p.Id == id);
        
        ViewBag.Nickname = nickname;
        return View(presentation);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string title, string nickname)
    {
        var presentation = new Presentation
        {
            Title = title,
            CreatorNickname = nickname,
            Slides = new List<Slide> { new Slide { Order = 0 } }
        };
        
        _context.Presentations.Add(presentation);
        await _context.SaveChangesAsync();
        
        return RedirectToAction("Edit", new { id = presentation.Id, nickname });
    }

    // public async Task<IActionResult> Edit(string id, string nickname)
    // {
    //     var presentation = await _context.Presentations
    //         .Include(p => p.Slides)
    //         .ThenInclude(s => s.TextBlocks)
    //         .FirstOrDefaultAsync(p => p.Id == id);
            
    //     if (presentation == null)
    //     {
    //         return NotFound();
    //     }
        
    //     ViewBag.Nickname = nickname;
    //     ViewBag.PresentationId = id;
    //     return View(presentation);
    // }

    public async Task<IActionResult> Present(string id)
    {
        var presentation = await _context.Presentations
            .Include(p => p.Slides)
            .ThenInclude(s => s.TextBlocks)
            .FirstOrDefaultAsync(p => p.Id == id);
            
        if (presentation == null)
        {
            return NotFound();
        }
        
        return View(presentation);
    }

    [HttpPost]
    public async Task<IActionResult> AddSlide(string presentationId)
    {
        var presentation = await _context.Presentations
            .Include(p => p.Slides)
            .FirstOrDefaultAsync(p => p.Id == presentationId);
            
        if (presentation != null)
        {
            var newSlide = new Slide
            {
                Order = presentation.Slides.Count,
                PresentationId = presentationId
            };
            
            _context.Slides.Add(newSlide);
            await _context.SaveChangesAsync();
        }
        
        return RedirectToAction("Edit", new { id = presentationId });
    }
}