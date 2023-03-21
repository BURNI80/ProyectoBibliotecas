using Microsoft.AspNetCore.Mvc;
using ProyectoBibliotecas.Models;
using System.Diagnostics;

namespace ProyectoBibliotecas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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

        public IActionResult LandingPage()
        {
            if (HttpContext.User.Identity.Name == null)
            {
                return View();
            }
            else{
                return RedirectToAction("IndexBibliotecas", "Bibliotecas");
            }
        }
    }
}