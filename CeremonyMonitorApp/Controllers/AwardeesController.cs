using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CeremonyMonitorApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CeremonyMonitorApp.Controllers
{
    [SessionAuthorize("Pf")]
    public class AwardeesController : Controller
    {
        private readonly AppDbContext _context;

        public AwardeesController(AppDbContext context)
        {
            _context = context;
        }

        // =========================================================
        // GET: /Awardee
        // Menampilkan nominasi yang dibuat oleh user yang sedang login
        // =========================================================
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var myAwardees = await _context.Awardees
                .AsNoTracking()
                .Include(a => a.Employee)
                .Include(a => a.Ceremony)
                .Where(a => a.SubmittedById == userId.Value)
                .OrderByDescending(a => a.SubmittedAt)
                .ToListAsync();

            return View(myAwardees);
        }


        // =========================================================
        // GET: /Awardee/Create
        // Menampilkan form nominasi
        // =========================================================
        public async Task<IActionResult> Create()
        {
            var departmentId = HttpContext.Session.GetInt32("UserDepartmentId");

            if (departmentId == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            await PrepareCreateViewAsync(departmentId.Value);

            return View();
        }


        // =========================================================
        // POST: /Awardee/Create
        // Menyimpan nominasi baru
        // =========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            int employeeId,
            string title,
            string reason)
        {
            // -----------------------------------------------------
            // 1. Ambil UserId dari Session
            // -----------------------------------------------------
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }


            // -----------------------------------------------------
            // 2. Ambil DepartmentId dari Session
            // -----------------------------------------------------
            var departmentId = HttpContext.Session.GetInt32("UserDepartmentId");

            if (departmentId == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }


            // -----------------------------------------------------
            // 3. Validasi input dasar
            // -----------------------------------------------------
            if (employeeId <= 0)
            {
                ModelState.AddModelError(
                    "employeeId",
                    "Silakan pilih karyawan yang akan dinominasikan."
                );
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                ModelState.AddModelError(
                    "title",
                    "Judul nominasi wajib diisi."
                );
            }

            if (string.IsNullOrWhiteSpace(reason))
            {
                ModelState.AddModelError(
                    "reason",
                    "Alasan nominasi wajib diisi."
                );
            }


            // -----------------------------------------------------
            // 4. Cari Employee dan pastikan berasal dari department
            //    user yang sedang login
            // -----------------------------------------------------
            var employee = await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e =>
                    e.Id == employeeId &&
                    e.DepartmentId == departmentId.Value
                );

            if (employee == null)
            {
                ModelState.AddModelError(
                    "employeeId",
                    "Karyawan yang dipilih tidak valid atau bukan berasal dari department Anda."
                );
            }


            // -----------------------------------------------------
            // 5. Cari Ceremony bulan berjalan
            // -----------------------------------------------------
            var currentCeremony = await GetCurrentCeremonyAsync();

            if (currentCeremony == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Tidak ada ceremony yang tersedia untuk bulan berjalan."
                );
            }


            // -----------------------------------------------------
            // 6. Kalau ada error, isi ulang data untuk View
            // -----------------------------------------------------
            if (!ModelState.IsValid)
            {
                await PrepareCreateViewAsync(departmentId.Value);
                ViewBag.Title = title;
                ViewBag.Reason = reason;
                ViewBag.SelectedEmployeeId = employeeId;
                return View();
            }


            // -----------------------------------------------------
            // 7. Buat Awardee baru
            // -----------------------------------------------------
            var awardee = new Awardee
            {
                CeremonyId = currentCeremony!.Id,

                NominatingDepartmentId = departmentId.Value,

                EmployeeId = employee!.Id,

                Title = title.Trim(),

                Reason = reason.Trim(),

                SubmittedById = userId.Value,

                Stage = ApprovalStage.Submitted,

                SubmittedAt = DateTime.Now
            };


            // -----------------------------------------------------
            // 8. Simpan ke database
            // -----------------------------------------------------
            _context.Awardees.Add(awardee);

            await _context.SaveChangesAsync();


            // -----------------------------------------------------
            // 9. Setelah berhasil → kembali ke Index
            // -----------------------------------------------------
            return RedirectToAction(nameof(Index));
        }


        // =========================================================
        // Helper:
        // Menyiapkan data yang dibutuhkan oleh halaman Create
        // =========================================================
        private async Task PrepareCreateViewAsync(int departmentId)
        {
            // -----------------------------------------------------
            // Employee hanya dari department user
            // -----------------------------------------------------
            var employees = await _context.Employees
                .AsNoTracking()
                .Where(e => e.DepartmentId == departmentId)
                .OrderBy(e => e.FullName)
                .ToListAsync();

            ViewBag.EmployeeId = new SelectList(
                employees,
                "Id",
                "FullName"
            );


            // -----------------------------------------------------
            // Ceremony bulan berjalan
            // -----------------------------------------------------
            var currentCeremony = await GetCurrentCeremonyAsync();

            ViewBag.CurrentCeremony = currentCeremony;

            ViewBag.NoCeremonyAvailable = currentCeremony == null;
        }


        // =========================================================
        // Helper:
        // Mengambil ceremony Scheduled terdekat pada bulan berjalan
        // =========================================================
        private async Task<Ceremony?> GetCurrentCeremonyAsync()
        {
            var today = DateTime.Today;

            return await _context.Ceremonies
                .AsNoTracking()
                .Where(c =>
                    c.Status == CeremonyStatus.Scheduled &&
                    c.ScheduledDate.Year == today.Year &&
                    c.ScheduledDate.Month == today.Month
                )
                .OrderBy(c => c.ScheduledDate)
                .FirstOrDefaultAsync();
        }
    }
}