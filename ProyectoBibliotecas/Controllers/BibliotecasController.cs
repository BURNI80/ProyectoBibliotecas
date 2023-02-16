using Microsoft.AspNetCore.Mvc;

namespace ProyectoBibliotecas.Controllers
{
    public class BibliotecasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
