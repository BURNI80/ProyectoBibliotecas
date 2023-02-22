using Microsoft.AspNetCore.Mvc;

namespace ProyectoBibliotecas.Controllers
{
    public class BibliotecasController : Controller
    {
        public IActionResult IndexBibliotecas()
        {
            return View();
        }

        public IActionResult DetailsBiblioteca()
        {
            return View();
        }
    }
}
