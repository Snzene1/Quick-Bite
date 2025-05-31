using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quick_Bite.Data;
using Quick_Bite.Models;
using Quick_Bite.Models.ViewModels;
using System.Diagnostics;
using System.Linq;

namespace Quick_Bite.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly QuickContext _context;

        public HomeController(ILogger<HomeController> logger, QuickContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var viewModel = new HomeViewModel
            {
                PopularRecipes = _context.Recipes
                    .OrderByDescending(r => r.DownloadCount)
                    .Take(3)
                    .ToList()
            };
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
