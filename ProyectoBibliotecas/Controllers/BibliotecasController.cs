using Microsoft.AspNetCore.Mvc;
using ProyectoBibliotecas.Repositorys;

namespace ProyectoBibliotecas.Controllers
{
    public class BibliotecasController : Controller
    {
        private BibliotecasRepository repo;

        public BibliotecasController(BibliotecasRepository repo)
        {
            this.repo = repo;
        }

        public IActionResult IndexBibliotecas()
        {
            return View(this.repo.GetBibliotecas());
        }

        [HttpPost]
        public IActionResult IndexBibliotecas(string search)
        {
            return View(this.repo.SearchBiblioteca(search));
        }

        [HttpGet]
        public IActionResult DetailsBiblioteca(int id)
        {
            ViewData["LISTALIBROS"] = this.repo.GetLibrosBiblioteca(id);
            return View(this.repo.GetDatosBiblioteca(id));
        }

        [HttpPost]
        public IActionResult DetailsBiblioteca(int id, string input, char option)
        {
            ViewData["LISTALIBROS"] = this.repo.SearchLibroBiblioteca(id,input,option);
            return View(this.repo.GetDatosBiblioteca(id));
        }
    }
}
