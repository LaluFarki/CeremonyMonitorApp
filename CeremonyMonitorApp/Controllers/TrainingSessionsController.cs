
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CeremonyMonitorApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

public class TrainingSessionsController : Controller
{
    private readonly AppDbContext _context;

    public TrainingSessionsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: TRAININGSESSIONS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.TrainingSessions.ToListAsync());
    }

    // GET: TRAININGSESSIONS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var trainingsession = await _context.TrainingSessions
            .FirstOrDefaultAsync(m => m.Id == id);
        if (trainingsession == null)
        {
            return NotFound();
        }

        return View(trainingsession);
    }

    // GET: TRAININGSESSIONS/Create
    public IActionResult Create()
    {
        ViewBag.CeremonyId = new SelectList(_context.Ceremonies, "Id", "ScheduledDate");
        return View();
    }

    // POST: TRAININGSESSIONS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,CeremonyId,Date,Time,Location")] TrainingSession trainingsession)
    {
        if (ModelState.IsValid)
        {
            _context.Add(trainingsession);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.CeremonyId = new SelectList(_context.Ceremonies, "Id", "ScheduledDate", trainingsession.CeremonyId);
        return View(trainingsession);
    }

    // GET: TRAININGSESSIONS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var trainingsession = await _context.TrainingSessions.FindAsync(id);
        if (trainingsession == null)
        {
            return NotFound();
        }
        ViewBag.CeremonyId = new SelectList(_context.Ceremonies, "Id", "ScheduledDate");
        return View(trainingsession);
    }

    // POST: TRAININGSESSIONS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,CeremonyId,Date,Time,Location")] TrainingSession trainingsession)
    {
        if (id != trainingsession.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(trainingsession);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingSessionExists(trainingsession.Id))
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
        ViewBag.CeremonyId = new SelectList(_context.Ceremonies, "Id", "ScheduledDate", trainingsession.CeremonyId);
        return View(trainingsession);
    }

    // GET: TRAININGSESSIONS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var trainingsession = await _context.TrainingSessions
            .FirstOrDefaultAsync(m => m.Id == id);
        if (trainingsession == null)
        {
            return NotFound();
        }

        return View(trainingsession);
    }

    // POST: TRAININGSESSIONS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var trainingsession = await _context.TrainingSessions.FindAsync(id);
        if (trainingsession != null)
        {
            _context.TrainingSessions.Remove(trainingsession);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TrainingSessionExists(int? id)
    {
        return _context.TrainingSessions.Any(e => e.Id == id);
    }
}
