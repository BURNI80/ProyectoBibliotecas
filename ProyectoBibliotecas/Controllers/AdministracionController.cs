using Microsoft.AspNetCore.Mvc;

namespace ProyectoBibliotecas.Controllers
{
    public class AdministracionController : Controller
    {
        public IActionResult Perfil_Info()
        {
            return View();
        }
        public IActionResult EditPerfil()
        {
            return View();
        }

        public IActionResult Perfil_Comentarios()
        {
            return View();
        }

        public IActionResult Perfil_Reservas()
        {
            return View();
        }

        public IActionResult Lista_LibrosFavoritos()
        {
            return View();
        }

        public IActionResult Lista_LibrosLeidos()
        {
            return View();
        }

        public IActionResult Admin_Panel()
        {
            return View();
        }
    }
}
