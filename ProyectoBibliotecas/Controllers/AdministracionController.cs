using Microsoft.AspNetCore.Mvc;
using ProyectoBibliotecas.Extensions;
using ProyectoBibliotecas.Models;
using ProyectoBibliotecas.Repositorys;

namespace ProyectoBibliotecas.Controllers
{
    public class AdministracionController : Controller
    {
        private BibliotecasRepository repo;

        public AdministracionController(BibliotecasRepository repo)
        {
            this.repo = repo;
        }

        public IActionResult Perfil_Info(string id)
        {
            id = "22";
            ViewData["NCOMENTARIOS"] = this.repo.NComentarios(id);
            ViewData["NLEIDOS"] = this.repo.NLibrosLeidos(id);
            ViewData["NRESEÑAS"] = this.repo.NReseñas(id);
            return View();
        }
        public IActionResult EditPerfil()
        {
            return View();
        }

        public IActionResult Perfil_Comentarios(string id)
        {
            id = "22";
            return View(this.repo.GetComentariosUsuario(id));
        }

        public IActionResult Perfil_Reservas(string id)
        {
            id = "22";
            return View(this.repo.GetReservasUsuario(id));
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
