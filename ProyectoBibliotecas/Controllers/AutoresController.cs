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
    }
}
