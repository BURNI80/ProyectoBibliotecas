using Microsoft.AspNetCore.Mvc;

namespace ProyectoBibliotecas.Controllers
{
    public class AutoresController : Controller
    {
        public IActionResult IndexAutores()
        {
            return View();
        }
    }
}
