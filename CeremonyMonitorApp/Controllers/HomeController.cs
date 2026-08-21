using System.Diagnostics;
using CeremonyMonitorApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CeremonyMonitorApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger, AppDbContext context )
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Schedule()
        {
            return View();
        }

        public IActionResult MCChecklist()
        {
            return View();
        }

        public IActionResult PrayersSpeech()
        {
            return View();
        }

        public IActionResult Practice()
        {
            return View();
        }

        public IActionResult History()
        {
            return View();
        }

        public async Task<IActionResult> CreateCeremony()
        {
            ViewBag.Departments = await _context.Departments.ToListAsync();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
