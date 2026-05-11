using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VereinKasseScaffolding.Data;
using VereinKasseScaffolding.Models;
using VereinKasseScaffolding.NewFolder;

namespace VereinKasseScaffolding.Controllers
{
    public class HomeController : Controller
    {
        private readonly VereinContext _context;
        private readonly MitgliedService _mitgliedService;
        public HomeController(VereinContext context, MitgliedService mitgliedService)
        {
            _context = context;
            _mitgliedService = mitgliedService;
        }

        public IActionResult Index()
        {
            return View();
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
