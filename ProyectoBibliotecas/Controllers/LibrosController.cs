﻿using Microsoft.AspNetCore.Mvc;
using ProyectoBibliotecas.Extensions;
using ProyectoBibliotecas.Models;
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
        public ActionResult DetailsLibro(int orden, int id, string textoComentario, int rating, int idLibro)
        {
            if (orden != 0)
            {
                this.repo.LikeComentario(orden, id);

            }
            else
            {
                DateTime fecha = DateTime.Now;
                string dni = HttpContext.Session.GetObject<Usuario>("user").DNI_USUARIO;
                this.repo.PostComentario(idLibro, dni, fecha, textoComentario, rating);
                ViewData["VALORACIONES"] = this.repo.GetValoraciones(id);
                ViewData["COMENTARIOS"] = this.repo.GetComentarios(id);
                return View(this.repo.GetDatosLibro(idLibro));
            }
            return new EmptyResult();

        }
    }
}
