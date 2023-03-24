using Microsoft.AspNetCore.Mvc;
using ProyectoBibliotecas.Extensions;
using ProyectoBibliotecas.Models;
using ProyectoBibliotecas.Repositorys;
using System.Collections.Generic;
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
            ViewData["FECHASNO"] = GetDiasReservado(id, 1);
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
            ViewData["FECHASNO"] = GetDiasReservado(id, 0);
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


        [HttpPost]
        public List<string> GetDiasReservado(int id, int idBiblio)
        {
            List<Reserva> reservas = this.repo.GetResrevasLibro(id, idBiblio);
            List<string> resultado = new List<string>();
            foreach (Reserva reserva in reservas)
            {
                List<string> arr = this.repo.GetDaysBetween(reserva.FECHA_INICIO, reserva.FECHA_FIN);
                resultado.AddRange(arr);
            }
            return resultado;
        }

        [HttpPost]
        public void ReservarLibro(int idLibro, int idBiblio, DateTime fechaInicio, DateTime fechaFin)
        {
            string dni = HttpContext.User.Identity.Name;
            this.repo.CreateReserva(dni, idLibro, idBiblio, fechaInicio, fechaFin);
        }
    }
}
