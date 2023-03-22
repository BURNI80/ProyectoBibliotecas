using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoBibliotecas.Extensions;
using ProyectoBibliotecas.Filters;
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

        [AuthorizeUsers]
        public IActionResult Perfil_Info(string id)
        {
            id = HttpContext.User.Identity.Name;
            ViewData["NCOMENTARIOS"] = this.repo.NComentarios(id);
            ViewData["NLEIDOS"] = this.repo.NLibrosLeidos(id);
            ViewData["NRESEÑAS"] = this.repo.NReseñas(id);
            return View(this.repo.GetUsuario(id));
        }

        [AuthorizeUsers]
        public IActionResult EditPerfil()
        {
            string id = HttpContext.User.Identity.Name;
            return View(this.repo.GetUsuario(id));
        }

        [AuthorizeUsers]
        [HttpPost]
        public IActionResult EditPerfil(string nombre, string apellido, string email, int telefono ,string usuario)
        {
            string id = HttpContext.User.Identity.Name;
            this.repo.UpdateUsuario(id, nombre, apellido, email, telefono, usuario);
            return RedirectToAction("Perfil_Info", "Administracion");
        }

        [AuthorizeUsers]
        public IActionResult Perfil_Comentarios(string id)
        {
            id = HttpContext.User.Identity.Name;
            return View(this.repo.GetComentariosUsuario(id));
        }
        [AuthorizeUsers]
        [HttpPost]
        public void DeleteComentario(int idComentario)
        {
            this.repo.DeleteComentario(idComentario);
        }

        [AuthorizeUsers]
        public IActionResult Perfil_Reservas(string id)
        {
            id = HttpContext.User.Identity.Name;
            return View(this.repo.GetReservasUsuario(id));
        }


        public IActionResult Lista_LibrosFavoritos(string id, string token)
        {
            string dniUser = HttpContext.User.Identity.Name;
            if(id == null)
            {
                return RedirectToAction("Login", "Managed");
            }
            if (id.Equals(dniUser))
            {
                return View(this.repo.GetFavoritos(id));
            }
            else
            {
                Share share = this.repo.GetShare(id);
                if (share == null)
                {
                    return RedirectToAction("NoAccess", "Managed");
                }
                else
                {
                    if (share.TOKEN.Equals(token))
                    {
                        return View(this.repo.GetFavoritos(id));
                    }
                    else
                    {
                        return RedirectToAction("NoAccess", "Managed");
                    }
                }
            }
        }

        public IActionResult Lista_LibrosLeidos(string id , string? token)
        {
            string dniUser = HttpContext.User.Identity.Name;
            if (id == null)
            {
                return RedirectToAction("Login", "Managed");
            }
            if (id.Equals(dniUser))
            {
                return View(this.repo.GetFavoritos(id));
            }
            else
            {
                Share share = this.repo.GetShare(id);
                if (share == null)
                {
                    return RedirectToAction("NoAccess", "Managed");
                }
                else
                {
                    if (share.TOKEN.Equals(token))
                    {
                        return View(this.repo.GetFavoritos(id));
                    }
                    else
                    {
                        return RedirectToAction("NoAccess", "Managed");
                    }
                }
            }
        }

        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        public IActionResult Admin_Panel()
        {
            return View();
        }

        [AuthorizeUsers]
        [HttpPost]
        public string Share(string dni, string path)
        {
            string token = this.repo.GenerateToken();
            Share realToken = this.repo.GetToken(dni,token);
            return path + "?token=" + realToken.TOKEN;
        }

        [Authorize]
        [HttpPost]
        public void EliminarReserva(int id)
        {
            this.repo.DeleteReserva(id);
        }

        public IActionResult Bibliotecas()
        {
            return View(this.repo.GetBibliotecas());
        }

        [HttpPost]
        public void EliminarBiblioteca(int id)
        {
            this.repo.DeleteBiblioteca(id);
        }

        public IActionResult NuevaBiblioteca()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NuevaBiblioteca(string nombre, string direccion, int telefono, string web, TimeSpan hora_apertura, TimeSpan hora_cierre, IFormFile imagen)
        {
            this.repo.AddBiblio(nombre, direccion, telefono, web, hora_apertura, hora_cierre, imagen);
            return RedirectToAction("Bibliotecas","Administracion");
        }

        public IActionResult EditarBiblioteca(int id)
        {
            return View(this.repo.GetDatosBiblioteca(id));
        }

        [HttpPost]
        public IActionResult EditarBiblioteca(int id,string nombre, string direccion, int telefono, string web, TimeSpan hora_apertura, TimeSpan hora_cierre, IFormFile imagen)
        {
            this.repo.UpdateBiblio(id,nombre, direccion, telefono, web, hora_apertura, hora_cierre, imagen);
            return RedirectToAction("Bibliotecas", "Administracion");
        }






        public IActionResult Libros()
        {
            return View(this.repo.GetLibrosTodos());
        }

        [HttpPost]
        public void EliminarLibro(int id)
        {
            this.repo.DeleteLibro(id);
        }

        public IActionResult NuevoLibro()
        {
            ViewData["AUTORES"] = this.repo.GetAutores();
            return View();
        }

        [HttpPost]
        public IActionResult NuevoLibro(string nombre, int numpag, IFormFile imagen, string urlcompra, string descripcion, string idioma, DateTime fecha_publicacion,int idautor)
        {
            this.repo.AddLibro(nombre, numpag, imagen, urlcompra, descripcion, idioma, fecha_publicacion,idautor);
            return RedirectToAction("Libros", "Administracion");
        }

        public IActionResult EditarLibro(int id)
        {
            ViewData["AUTORES"] = this.repo.GetAutores();
            return View(this.repo.GetDatosLibroDef(id));
        }

        [HttpPost]
        public IActionResult EditarLibro(int id, string nombre, int numpag, IFormFile imagen, string urlcompra, string descripcion, string idioma, DateTime fecha_publicacion, int idautor)
        {
            this.repo.UpdateLibro(id,nombre, numpag, imagen, urlcompra, descripcion, idioma, fecha_publicacion, idautor);
            return RedirectToAction("Libros", "Administracion");
        }





    }
}
