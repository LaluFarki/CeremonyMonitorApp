using System.Reflection.Metadata.Ecma335;
using CeremonyMonitorApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


public class CreateCeremonyWizardDto
{
    //tanggal pelaksanaan upaa=
    public DateTime ScheduledDate { get; set; }

    //Id departemen Pelaksana
    public int DepartmentId { get; set; }

    //Daftar item rundown Mc 
    public List<RundownItemDto> RundownItems { get; set; } = new();

    //teks Doa 
    public string? prayertext { get; set; }

    // Properti baru: bernilai true jika HRD mengonfirmasi ingin memaksa simpan
    public bool Force { get; set; }
}

public class RundownItemDto
{
    // Judul Kegiatan 
    public string Title { get; set; } = string.Empty;

    //Kategori kegiatan 
    public string Category { get; set; } = string.Empty;
}


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


     // =========================================================
    // POST: /Ceremonies/CreateWizard
    // Menyimpan seluruh data upacara, rundown, dan doa dari Wizard
    // =========================================================
    [HttpPost]
    public async Task<IActionResult> CreateWizard([FromBody] CreateCeremonyWizardDto dto)
    {
        //1.Valdiasi awal jika data yg dikirim kosong
        if (dto == null)
        {
            return BadRequest("Data Pendaftaran Tidak Valid");
        }

        // Jika parameter Force bernilai false, kita lakukan pengecekan duplikasi bulan
        if (!dto.Force)
        {
            var targetMonth = dto.ScheduledDate.Month;
            var targetYear = dto.ScheduledDate.Year;
            // Cek apakah ada upacara di database dengan bulan & tahun yang sama
            var duplicateExists = await _context.Ceremonies
                .AnyAsync(c => c.ScheduledDate.Month == targetMonth && c.ScheduledDate.Year == targetYear);
            if (duplicateExists)
            {
                var monthName = dto.ScheduledDate.ToString("MMM yyyy");
                // Mengembalikan pesan peringatan ke frontend tanpa menyimpan ke database
                return Json(new
                {
                    success = false,
                    duplicateWarning = true,
                    message = $"The Ceremony In {monthName} is Already Exist , Are you Sure To Add it?"
                });
            }
        }
        //2. Buat objek Upacara (Ceremony Baru)
        var ceremony = new Ceremony
        {
            DepartmentId = dto.DepartmentId,
            ScheduledDate = dto.ScheduledDate,
            Status = CeremonyStatus.Scheduled
        };

        //menambahkan antrian ke database
        _context.Ceremonies.Add(ceremony);

        //Simpan perubahan pertama , SUpayaA SQL server memuat ID oromatis
        await _context.SaveChangesAsync();

        //3. Masukkan item rundown MC ke tabel Db MCChecklis
        int index = 0;
        foreach (var item in dto.RundownItems)
        {
            var mccItem = new MCChecklistItem
            {
                CeremonyId = ceremony.Id,
                Title = item.Title,
                ScripText = item.Category,
                OrderIndex = index++,
                IsCompleted =  false
            };
            _context.MCChecklistItems.Add(mccItem);
        }

        // 4. Memasukkan Teks Doa ke Tabel nya
        if (!string.IsNullOrWhiteSpace(dto.prayertext))
        {
            var prayer = new PrayerText()
            {
                CeremonyId = ceremony.Id,
                Title = $"Prayer for {ceremony.ScheduledDate: MMMM yyyy}",
                Text = dto.prayertext.Trim(),
                UpdatedAt =  DateTime.Now
            };
            _context.PrayerTexts.Add(prayer);
        }

        //5 SImoan  seluruh detail rundown ke Db
        await _context.SaveChangesAsync();
        
    // Mengembalikan respons sukses berupa JSON ke JavaScript di frontend
    return Json( new {success = true, ceremonyId = ceremony.Id});
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