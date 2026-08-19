
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CeremonyMonitorApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

public class PrayerTextsController : Controller
{
    private readonly AppDbContext _context;

    public PrayerTextsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: PRAYERTEXTS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.PrayerTexts.ToListAsync());
    }

    // GET: PRAYERTEXTS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var prayertext = await _context.PrayerTexts
            .FirstOrDefaultAsync(m => m.Id == id);
        if (prayertext == null)
        {
            return NotFound();
        }

        return View(prayertext);
    }

    // GET: PRAYERTEXTS/Create
    public IActionResult Create()
    {
        ViewBag.CeremonyId = new SelectList(_context.Ceremonies, "Id", "Name");
        return View();
    }

    // POST: PRAYERTEXTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,CeremonyId,Title,Text,UpdatedAt")] PrayerText prayertext)
    {
        if (ModelState.IsValid)
        {
            _context.Add(prayertext);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.CeremonyId = new SelectList(_context.Ceremonies, "Id", "Name", prayertext.CeremonyId);
        return View(prayertext);
    }

    // GET: PRAYERTEXTS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var prayertext = await _context.PrayerTexts.FindAsync(id);
        if (prayertext == null)
        {
            return NotFound();
        }
        ViewBag.CeremonyId = new SelectList(_context.Ceremonies, "Id", "Name", prayertext.CeremonyId);
        return View(prayertext);
    }

    // POST: PRAYERTEXTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,CeremonyId,Title,Text,UpdatedAt")] PrayerText prayertext)
    {
        if (id != prayertext.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(prayertext);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrayerTextExists(prayertext.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.CeremonyId = new SelectList(_context.Ceremonies, "Id", "Name", prayertext.CeremonyId);
        return View(prayertext);
    }

    // GET: PRAYERTEXTS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var prayertext = await _context.PrayerTexts
            .FirstOrDefaultAsync(m => m.Id == id);
        if (prayertext == null)
        {
            return NotFound();
        }

        return View(prayertext);
    }

    // POST: PRAYERTEXTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var prayertext = await _context.PrayerTexts.FindAsync(id);
        if (prayertext != null)
        {
            _context.PrayerTexts.Remove(prayertext);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PrayerTextExists(int? id)
    {
        return _context.PrayerTexts.Any(e => e.Id == id);
    }
}
