
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CeremonyMonitorApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

public class RoleAssignmentsController : Controller
{
    private readonly AppDbContext _context;

    public RoleAssignmentsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: ROLEASSIGNMENTS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.RoleAssignments.ToListAsync());
    }

    // GET: ROLEASSIGNMENTS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var roleassignment = await _context.RoleAssignments
            .FirstOrDefaultAsync(m => m.Id == id);
        if (roleassignment == null)
        {
            return NotFound();
        }

        return View(roleassignment);
    }

    // GET: ROLEASSIGNMENTS/Create
    public IActionResult Create()
    {
        ViewBag.CeremonyId = new SelectList(_context.Ceremonies, "Id", "ScheduledDate");
        ViewBag.EmployeeId = new SelectList(_context.Employees, "Id", "FullName");
        return View();
    }

    // POST: ROLEASSIGNMENTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,CeremonyId,EmployeeId,RoleType,Submitted")] RoleAssignment roleassignment)
    {
        if (ModelState.IsValid)
        {
            _context.Add(roleassignment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.CeremonyId = new SelectList(_context.Ceremonies, "Id", "ScheduledDate", roleassignment.CeremonyId);
        ViewBag.EmployeeId = new SelectList(_context.Employees, "Id", "FullName", roleassignment.EmployeeId);
        return View(roleassignment);
    }

    // GET: ROLEASSIGNMENTS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var roleassignment = await _context.RoleAssignments.FindAsync(id);
        if (roleassignment == null)
        {
            return NotFound();
        }
        ViewBag.CeremonyId = new SelectList(_context.Ceremonies, "Id", "ScheduledDate", roleassignment.CeremonyId);
        ViewBag.EmployeeId = new SelectList(_context.Employees, "Id", "FullName", roleassignment.CeremonyId);
        return View(roleassignment);
    }

    // POST: ROLEASSIGNMENTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,CeremonyId,EmployeeId,RoleType,Submitted")] RoleAssignment roleassignment)
    {
        if (id != roleassignment.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(roleassignment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleAssignmentExists(roleassignment.Id))
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
        ViewBag.CeremonyId = new SelectList(_context.Ceremonies, "Id", "ScheduledDate", roleassignment.CeremonyId);
        ViewBag.EmployeeId = new SelectList(_context.Employees, "Id", "FullName", roleassignment.EmployeeId);
        return View(roleassignment);
    }

    // GET: ROLEASSIGNMENTS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var roleassignment = await _context.RoleAssignments
            .FirstOrDefaultAsync(m => m.Id == id);
        if (roleassignment == null)
        {
            return NotFound();
        }

        return View(roleassignment);
    }

    // POST: ROLEASSIGNMENTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var roleassignment = await _context.RoleAssignments.FindAsync(id);
        if (roleassignment != null)
        {
            _context.RoleAssignments.Remove(roleassignment);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool RoleAssignmentExists(int? id)
    {
        return _context.RoleAssignments.Any(e => e.Id == id);
    }
}
