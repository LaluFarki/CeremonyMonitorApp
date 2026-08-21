using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CeremonyMonitorApp.Models;

namespace CeremonyMonitorApp.Controllers
{
    [SessionAuthorize] // Relaxed role constraint to allow any logged-in user to test
public class AwardeeApprovalController : Controller
{
    private readonly AppDbContext _context;

    public AwardeeApprovalController(AppDbContext context)
    {
        _context = context;
    }

    // GET: /AwardeeApproval — antrian sesuai role yang login
    public async Task<IActionResult> Index()
    {
        var role = HttpContext.Session.GetString("UserRole") ?? "HrAdmin"; // Fallback to HrAdmin for testing
        //ternary operator
        var targetStage = role == "HrManager" ? ApprovalStage.HrManagerFinal : ApprovalStage.Submitted;

        var queue = await _context.Awardees
            .Include(a => a.Employee)
            .Include(a => a.NominatingDepartment)
            .Include(a => a.Ceremony)
            .Where(a => a.Stage == targetStage)
            .OrderBy(a => a.Ceremony!.ScheduledDate)
            .ToListAsync();

        ViewBag.Role = role;
        return View(queue);
    }

    // POST: /AwardeeApproval/Approve/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id, string? notes)
    {
        var awardee = await _context.Awardees.FindAsync(id);
        if (awardee == null) return NotFound();

        var role = HttpContext.Session.GetString("UserRole");
        var userId = HttpContext.Session.GetInt32("UserId")!.Value;

        if (role == "HrAdmin" && awardee.Stage == ApprovalStage.Submitted)
        {
            awardee.Stage = ApprovalStage.HrManagerFinal;
            awardee.HrAdminNotes = notes;
            awardee.ReviewedByHrAdminId = userId;
        }
        else if (role == "HrManager" && awardee.Stage == ApprovalStage.HrManagerFinal)
        {
            awardee.Stage = ApprovalStage.Approved;
            awardee.HrManagerNotes = notes;
            awardee.ReviewedByHrManagerId = userId;
        }
        else
        {
            return RedirectToAction("AccessDenied", "Account");
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // POST: /AwardeeApproval/Reject/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(int id, string notes)
    {
        var awardee = await _context.Awardees.FindAsync(id);
        if (awardee == null) return NotFound();

        if (string.IsNullOrWhiteSpace(notes))
        {
            TempData["RejectError"] = "Catatan alasan penolakan wajib diisi.";
            return RedirectToAction(nameof(Index));
        }

        var role = HttpContext.Session.GetString("UserRole");
        var userId = HttpContext.Session.GetInt32("UserId")!.Value;

        awardee.Stage = ApprovalStage.Rejected;

        if (role == "HrAdmin")
        {
            awardee.HrAdminNotes = notes;
            awardee.ReviewedByHrAdminId = userId;
        }
        else if (role == "HrManager")
        {
            awardee.HrManagerNotes = notes;
            awardee.ReviewedByHrManagerId = userId;
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
}