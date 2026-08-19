using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CeremonyMonitorApp.Models;
using Microsoft.AspNetCore.Authorization;

public class AccountController : Controller
{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Account/Login
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    // POST: Account/Login
    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await _context.AppUsers
            // Include the Employee navigation property to access the related Employee entity
            .Include(u => u.Employee)
            //buat nemuin email pertama kali yang cocok dengan email yang diinputkan
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            ViewBag.ErrorMessage = "Email atau password salah.";
            return View();
        }

        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("UserRole", user.Role.ToString());
        HttpContext.Session.SetString("UserName", user.Employee?.FullName ?? user.Email);
        if (user.DepartmentId.HasValue)
            HttpContext.Session.SetInt32("UserDepartmentId", user.DepartmentId.Value);

        return RedirectToAction("Index", "Ceremonies");
    }
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }

    // POST: Account/Logout
    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}