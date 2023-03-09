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

        public IActionResult DetailsBiblioteca(int id)
        {
            return View(this.repo.GetDatosBiblioteca(id));
        }
    }
}
