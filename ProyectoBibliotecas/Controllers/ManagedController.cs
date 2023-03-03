using Microsoft.AspNetCore.Mvc;

namespace ProyectoBibliotecas.Controllers
{
    public class ManagedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
