
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CeremonyMonitorApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
public class MCChecklistItemsController : Controller
{
    private readonly AppDbContext _context;

    public MCChecklistItemsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: MCCHECKLISTITEMS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.MCChecklistItems.ToListAsync());
    }

    // GET: MCCHECKLISTITEMS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var mcchecklistitem = await _context.MCChecklistItems
            .FirstOrDefaultAsync(m => m.Id == id);
        if (mcchecklistitem == null)
        {
            return NotFound();
        }

        return View(mcchecklistitem);
    }

    // GET: MCCHECKLISTITEMS/Create
    public IActionResult Create()
    {
        ViewBag.CeremonyId = new SelectList(_context.Ceremonies, "Id", "ScheduledDate");
        return View();
    }

    // POST: MCCHECKLISTITEMS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,CeremonyId,OrderIndex,Title,ScripText,IsCompleted")] MCChecklistItem mcchecklistitem)
    {
        if (ModelState.IsValid)
        {
            _context.Add(mcchecklistitem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.CeremonyId = new SelectList(_context.Ceremonies, "Id", "ScheduledDate", mcchecklistitem.CeremonyId);
        return View(mcchecklistitem);
    }

    // GET: MCCHECKLISTITEMS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var mcchecklistitem = await _context.MCChecklistItems.FindAsync(id);
        if (mcchecklistitem == null)
        {
            return NotFound();
        }
        ViewBag.CeremonyId = new SelectList(_context.Ceremonies, "Id", "ScheduledDate");
        return View(mcchecklistitem);
    }

    // POST: MCCHECKLISTITEMS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,CeremonyId,OrderIndex,Title,ScripText,IsCompleted")] MCChecklistItem mcchecklistitem)
    {
        if (id != mcchecklistitem.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(mcchecklistitem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MCChecklistItemExists(mcchecklistitem.Id))
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
        ViewBag.CeremonyId = new SelectList(_context.Ceremonies, "Id","ScheduledDate", mcchecklistitem.CeremonyId);
        return View(mcchecklistitem);
    }

    // GET: MCCHECKLISTITEMS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var mcchecklistitem = await _context.MCChecklistItems
            .FirstOrDefaultAsync(m => m.Id == id);
        if (mcchecklistitem == null)
        {
            return NotFound();
        }

        return View(mcchecklistitem);
    }

    // POST: MCCHECKLISTITEMS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var mcchecklistitem = await _context.MCChecklistItems.FindAsync(id);
        if (mcchecklistitem != null)
        {
            _context.MCChecklistItems.Remove(mcchecklistitem);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MCChecklistItemExists(int? id)
    {
        return _context.MCChecklistItems.Any(e => e.Id == id);
    }
}
