using Microsoft.AspNetCore.Mvc;
using ProyectoBibliotecas.Extensions;
using ProyectoBibliotecas.Models;
using ProyectoBibliotecas.Repositorys;
using System.Net;

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
            ViewData["BIBLIOTECAS"] = this.repo.GetLibroDisponible(id);
            string dni;
            if (HttpContext.User.Identity.IsAuthenticated == false)
            {
                dni = null;
                ViewData["LISTADESEOS"] = -2;
            }
            else
            {
                dni = HttpContext.User.Identity.Name;
                ViewData["LISTADESEOS"] = this.repo.LibroDeseo(id, dni);
            }
            ViewData["COMENTARIOS"] = this.repo.GetComentarios(id, dni);
            ViewData["DNI"] = dni;
            return View(this.repo.GetDatosLibro(id));
        }

        [HttpPost]
        public ActionResult DetailsLibro(int orden, int id, string textoComentario, int rating, int idLibro)
        {
            string dni = HttpContext.User.Identity.Name;
            ViewData["LISTADESEOS"] = this.repo.LibroDeseo(id, dni);
            ViewData["BIBLIOTECAS"] = this.repo.GetLibroDisponible(id);
            if (orden != 0)
            {
                this.repo.LikeComentario(orden, id, dni);

            }
            else
            {
                DateTime fecha = DateTime.Now;
                this.repo.PostComentario(idLibro, dni, fecha, textoComentario, rating);
                ViewData["VALORACIONES"] = this.repo.GetValoraciones(id);
                ViewData["COMENTARIOS"] = this.repo.GetComentarios(id, dni);
                ViewData["DNI"] = dni;
                return View(this.repo.GetDatosLibro(idLibro));
            }
            return new EmptyResult();

        }


        [HttpPost]
        public void AddListaLibro(string dni, int idLibro, int orden)
        {
            this.repo.AddListaLibro(dni, idLibro, orden);
        }
    }
}
