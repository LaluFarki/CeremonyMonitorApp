using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CeremonyMonitorApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

public class SpeechesController : Controller
{
    private readonly AppDbContext _context;

    public SpeechesController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Speeches.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var speech = await _context.Speeches.FirstOrDefaultAsync(m => m.Id == id);
        if (speech == null) return NotFound();
        return View(speech);
    }

    public IActionResult Create()
    {
        ViewBag.CeremonyId = new SelectList(_context.Ceremonies, "Id", "ScheduledDate");
        ViewBag.AttributedToId = new SelectList(_context.Employees, "Id", "FullName");
        ViewBag.InputById = new SelectList(_context.AppUsers, "Id", "Email");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,CeremonyId,AttributedToId,InputById,TextJapanese,TextIndonesian")] Speech speech)
    {
        if (ModelState.IsValid)
        {
            speech.UpdatedAt = DateTime.Now;
            _context.Add(speech);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.CeremonyId = new SelectList(_context.Ceremonies, "Id", "ScheduledDate", speech.CeremonyId);
        ViewBag.AttributedToId = new SelectList(_context.Employees, "Id", "FullName", speech.AttributedToId);
        ViewBag.InputById = new SelectList(_context.AppUsers, "Id", "Email", speech.InputById);
        return View(speech);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var speech = await _context.Speeches.FindAsync(id);
        if (speech == null) return NotFound();
        ViewBag.CeremonyId = new SelectList(_context.Ceremonies, "Id", "ScheduledDate", speech.CeremonyId);
        ViewBag.AttributedToId = new SelectList(_context.Employees, "Id", "FullName", speech.AttributedToId);
        ViewBag.InputById = new SelectList(_context.AppUsers, "Id", "Email", speech.InputById);
        return View(speech);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,CeremonyId,AttributedToId,InputById,TextJapanese,TextIndonesian")] Speech speech)
    {
        if (id != speech.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                speech.UpdatedAt = DateTime.Now;
                _context.Update(speech);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpeechExists(speech.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.CeremonyId = new SelectList(_context.Ceremonies, "Id", "ScheduledDate", speech.CeremonyId);
        ViewBag.AttributedToId = new SelectList(_context.Employees, "Id", "FullName", speech.AttributedToId);
        ViewBag.InputById = new SelectList(_context.AppUsers, "Id", "Email", speech.InputById);
        return View(speech);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var speech = await _context.Speeches.FirstOrDefaultAsync(m => m.Id == id);
        if (speech == null) return NotFound();
        return View(speech);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var speech = await _context.Speeches.FindAsync(id);
        if (speech != null) _context.Speeches.Remove(speech);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SpeechExists(int? id)
    {
        return _context.Speeches.Any(e => e.Id == id);
    }
}