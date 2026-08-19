using CeremonyMonitorApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class CeremoniesController : Controller
{
    private readonly AppDbContext _context;

    public CeremoniesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: CEREMONYS
    public async Task<IActionResult> Index()
    {
        var ceremonies = await _context.Ceremonies
            .Include(c => c.Department)
            .ToListAsync();

        return View(ceremonies);
    }

    // GET: CEREMONYS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ceremony = await _context.Ceremonies
            .FirstOrDefaultAsync(m => m.Id == id);
        if (ceremony == null)
        {
            return NotFound();
        }

        return View(ceremony);
    }

    // GET: CEREMONYS/Create
    public IActionResult Create()
    {
        ViewBag.DepartmentId = new SelectList(_context.Departments, "Id", "Name");
        return View();
    }

    // POST: CEREMONYS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,DepartmentId,ScheduledDate")] Ceremony ceremony)
    {
        if (ModelState.IsValid)
        {
            _context.Add(ceremony);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.DepartmentId = new SelectList(_context.Departments, "Id", "Name", ceremony.DepartmentId);
        return View(ceremony);
    }

    // GET: CEREMONYS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ceremony = await _context.Ceremonies.FindAsync(id);
        if (ceremony == null)
        {
            return NotFound();
        }
        ViewBag.DepartmentId = new SelectList(_context.Departments, "Id", "Name", ceremony.DepartmentId);
        return View(ceremony);
    }

    // POST: CEREMONYS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,DepartmentId,ScheduledDate")] Ceremony ceremony)
    {
        if (id != ceremony.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(ceremony);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CeremonyExists(ceremony.Id))
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
        ViewBag.DepartmentId = new SelectList(_context.Departments, "Id", "Name", ceremony.DepartmentId);
        return View(ceremony);
        /*
         ViewBag.CeremonyId: Membuat dynamic property bernama CeremonyId pada objek ViewBag untuk dikirimkan ke tampilan (View).

        new SelectList(...): Membuat opsi pilihan drop-down yang berisi daftar data dari tabel Ceremonies.

        _context.Ceremonies: Sumber data daftar upacara (Ceremony).

        "Id": Value yang akan disimpan ke database saat form disubmit (<option value="Id">).

        "Name": Teks yang akan ditayangkan/dibaca pengguna pada layar drop-down.

        prayertext.CeremonyId: Menentukan nilai mana yang otomatis terpilih (selected) kembali jika validasi form gagal, sehingga user tidak perlu memilih ulang.

        */
    }

    // GET: CEREMONYS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ceremony = await _context.Ceremonies
            .FirstOrDefaultAsync(m => m.Id == id);
        if (ceremony == null)
        {
            return NotFound();
        }

        return View(ceremony);
    }

    // POST: CEREMONYS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var ceremony = await _context.Ceremonies.FindAsync(id);
        if (ceremony != null)
        {
            _context.Ceremonies.Remove(ceremony);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CeremonyExists(int? id)
    {
        return _context.Ceremonies.Any(e => e.Id == id);
    }
}