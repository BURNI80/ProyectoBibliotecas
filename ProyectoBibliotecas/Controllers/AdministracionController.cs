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
    }
}
