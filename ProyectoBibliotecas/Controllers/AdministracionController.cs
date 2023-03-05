using Microsoft.AspNetCore.Mvc;

namespace ProyectoBibliotecas.Controllers
{
    public class AdministracionController : Controller
    {
        public IActionResult Info()
        {
            return View();
        }
        public IActionResult EditPerfil()
        {
            return View();
        }

        public IActionResult Comentarios()
        {
            return View();
        }

        public IActionResult Reservas()
        {
            return View();
        }

        public IActionResult LibrosFavoritos()
        {
            return View();
        }

        public IActionResult LibrosLeidos()
        {
            return View();
        }
    }
}
