using Microsoft.AspNetCore.Mvc;
using ProyectoBibliotecas.Repositorys;

namespace ProyectoBibliotecas.Controllers
{
    public class AutoresController : Controller
    {
        private BibliotecasRepository repo;

        public AutoresController(BibliotecasRepository repository)
        {
            this.repo = repository;
        }

        public IActionResult IndexAutores()
        {
            return View(this.repo.GetAutores());
        }

        [HttpPost]
        public IActionResult IndexAutores(string search)
        {
            return View(this.repo.SearchAutor(search));
        }

        public IActionResult DetailsAutor(int id)
        {
            ViewData["LIBROS"] = this.repo.GetLibrosAutor(id);
            return View(this.repo.GetDatosAutor(id));
        }
        [HttpPost]
        public IActionResult DetailsAutor(int id, string input)
        {
            ViewData["LIBROS"] = this.repo.SearchLibroAutor(id, input);
            return View(this.repo.GetDatosAutor(id));
        }
    }
}
