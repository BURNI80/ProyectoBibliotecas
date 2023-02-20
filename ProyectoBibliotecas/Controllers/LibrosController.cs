using Microsoft.AspNetCore.Mvc;

namespace ProyectoBibliotecas.Controllers
{
    public class LibrosController : Controller
    {
        public IActionResult IndexLibros()
        {
            return View();
        }
    }
}
