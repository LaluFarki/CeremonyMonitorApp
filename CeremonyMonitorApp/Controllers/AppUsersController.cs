
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CeremonyMonitorApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

public class AppUsersController : Controller
{
    private readonly AppDbContext _context;

    public AppUsersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: APPUSERS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.AppUsers
            .Include(a => a.Employee)
            .Include(a => a.Department)
            .ToListAsync());
    }

    // GET: APPUSERS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var appuser = await _context.AppUsers
            .Include(a => a.Employee)
            .Include(a => a.Department)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (appuser == null)
        {
            return NotFound();
        }

        return View(appuser);
    }

    // GET: APPUSERS/Create
    public IActionResult Create()
    {
    ViewBag.DepartmentId = new SelectList(_context.Departments, "Id", "Name");
           ViewBag.EmployeeId = new SelectList(_context.Employees, "Id", "FullName");
        return View();
    }

    // POST: APPUSERS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,EmployeeId,Role,DepartmentId,Email")] AppUser appuser, string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            ModelState.AddModelError(string.Empty, "Password is required.");
        }

        if (ModelState.IsValid)
        {
            appuser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            _context.Add(appuser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.EmployeeId = new SelectList(_context.Employees, "Id", "FullName", appuser.EmployeeId);
        ViewBag.DepartmentId = new SelectList(_context.Departments, "Id", "Name", appuser.DepartmentId);
        return View(appuser);
    }

    // GET: APPUSERS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var appuser = await _context.AppUsers.FindAsync(id);
        if (appuser == null)
        {
            return NotFound();
        }
        ViewBag.DepartmentId = new SelectList(_context.Departments, "Id", "Name", appuser.DepartmentId);
        ViewBag.EmployeeId = new SelectList(_context.Employees, "Id", "FullName", appuser.EmployeeId);
        return View(appuser);
    }

    // POST: APPUSERS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,EmployeeId,Role,DepartmentId,Email")] AppUser appuser, string? password)
    {
        if (id != appuser.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                if (!string.IsNullOrEmpty(password))
                {
                    appuser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
                }
                else
                {
                    var existingUser = await _context.AppUsers.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                    if (existingUser != null)
                    {
                        appuser.PasswordHash = existingUser.PasswordHash;
                    }
                }

                _context.Update(appuser);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppUserExists(appuser.Id))
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
        ViewBag.EmployeeId = new SelectList(_context.Employees, "Id", "FullName", appuser.EmployeeId);
        ViewBag.DepartmentId = new SelectList(_context.Departments, "Id", "Name", appuser.DepartmentId);
        return View(appuser);
    }

    // GET: APPUSERS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var appuser = await _context.AppUsers
            .Include(a => a.Employee)
            .Include(a => a.Department)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (appuser == null)
        {
            return NotFound();
        }

        return View(appuser);
    }

    // POST: APPUSERS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var appuser = await _context.AppUsers.FindAsync(id);
        if (appuser != null)
        {
            _context.AppUsers.Remove(appuser);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AppUserExists(int? id)
    {
        return _context.AppUsers.Any(e => e.Id == id);
    }
}
