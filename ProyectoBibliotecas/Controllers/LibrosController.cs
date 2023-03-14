using Microsoft.AspNetCore.Mvc;
using ProyectoBibliotecas.Repositorys;

namespace ProyectoBibliotecas.Controllers
{
    public class LibrosController : Controller
    {
        BibliotecasRepository repo;

        public LibrosController(BibliotecasRepository repo)
        {
            this.repo = repo;
        }

        public IActionResult IndexLibros()
        {
            return View(this.repo.SearchLibroBiblioteca(-1, null, 'T'));
        }

        [HttpPost]
        public IActionResult IndexLibros(string input, char option)
        {
            return View(this.repo.SearchLibroBiblioteca(-1, input, option));
        }


        public IActionResult DetailsLibro(int id)
        {
            ViewData["VALORACIONES"] = this.repo.GetValoraciones(id);
            ViewData["COMENTARIOS"] = this.repo.GetComentarios(id);
            return View(this.repo.GetDatosLibro(id));
        }
        [HttpPost]
        public void DetailsLibro(int orden,int id)
        {
            
        }
    }
}
