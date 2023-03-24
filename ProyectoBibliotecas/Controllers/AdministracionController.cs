using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoBibliotecas.Extensions;
using ProyectoBibliotecas.Filters;
using ProyectoBibliotecas.Helpers;
using ProyectoBibliotecas.Models;
using ProyectoBibliotecas.Repositorys;

namespace ProyectoBibliotecas.Controllers
{

    public class AdministracionController : Controller
    {
        private BibliotecasRepository repo;
        private HelperUploadFiles upload;

        public AdministracionController(BibliotecasRepository repo, HelperUploadFiles upload)
        {
            this.repo = repo;
            this.upload = upload;
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
        public IActionResult EditPerfil(string nombre, string apellido, string email, int telefono, string usuario)
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
                Compartido share = this.repo.GetShare(id);
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

        public IActionResult Lista_LibrosLeidos(string id, string? token)
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
                Compartido share = this.repo.GetShare(id);
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
            if (HttpContext.User.IsInRole("EDITOR"))
            {
                ViewData["BIBLIOTECAS"] = this.repo.GetBibliotecasEditables(HttpContext.User.Identity.Name);
            }
            if (HttpContext.User.IsInRole("ADMIN"))
            {
                ViewData["BIBLIOTECAS"] = this.repo.GetBibliotecasSimples();
            }
            return View();
        }

        [AuthorizeUsers]
        [HttpPost]
        public string Share(string dni, string path)
        {
            string token = this.repo.GenerateToken();
            Compartido realToken = this.repo.GetToken(dni, token);
            return path + "?token=" + realToken.TOKEN;
        }

        [Authorize]
        [HttpPost]
        public void EliminarReserva(int id)
        {
            this.repo.DeleteReserva(id);
        }







        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        public IActionResult Bibliotecas()
        {
            return View(this.repo.GetBibliotecas());
        }


        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        [HttpPost]
        public void EliminarBiblioteca(int id)
        {
            this.repo.DeleteBiblioteca(id);
        }


        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        public IActionResult NuevaBiblioteca()
        {
            return View();
        }


        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        [HttpPost]
        public async Task<IActionResult> NuevaBiblioteca(string nombre, string direccion, int telefono, string web, TimeSpan hora_apertura, TimeSpan hora_cierre, IFormFile imagen)
        {
            if (imagen == null)
            {
                this.repo.AddBiblio(nombre, direccion, telefono, web, hora_apertura, hora_cierre, null);

            }
            else
            {
                string fileName = imagen.FileName;
                await this.upload.UploadFileAsync(imagen, Folders.Bibliotecas);
                this.repo.AddBiblio(nombre, direccion, telefono, web, hora_apertura, hora_cierre, fileName.ToString());

            }
            return RedirectToAction("Bibliotecas", "Administracion");
        }


        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        public IActionResult EditarBiblioteca(int id)
        {
            return View(this.repo.GetDatosBiblioteca(id));
        }


        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        [HttpPost]
        public async Task<IActionResult> EditarBiblioteca(int id, string nombre, string direccion, int telefono, string web, TimeSpan hora_apertura, TimeSpan hora_cierre, IFormFile imagen, string nImagen)
        {
            if (imagen == null)
            {
                this.repo.UpdateBiblio(id, nombre, direccion, telefono, web, hora_apertura, hora_cierre, nImagen);
            }
            else
            {
                string fileName = imagen.FileName;
                await this.upload.UploadFileAsync(imagen, Folders.Bibliotecas);
                this.repo.UpdateBiblio(id, nombre, direccion, telefono, web, hora_apertura, hora_cierre, fileName);
            }
            return RedirectToAction("Bibliotecas", "Administracion");

        }





        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        public IActionResult Libros()
        {
            return View(this.repo.GetLibrosTodos());
        }


        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        [HttpPost]
        public void EliminarLibro(int id)
        {
            this.repo.DeleteLibro(id);
        }


        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        public IActionResult NuevoLibro()
        {
            ViewData["AUTORES"] = this.repo.GetAutores();
            return View();
        }


        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        [HttpPost]
        public async Task<IActionResult> NuevoLibro(string nombre, int numpag, IFormFile imagen, string urlcompra, string descripcion, string idioma, DateTime fecha_publicacion, int idautor)
        {
            if (imagen == null)
            {
                this.repo.AddLibro(nombre, numpag, null, urlcompra, descripcion, idioma, fecha_publicacion, idautor);
            }
            else
            {
                string fileName = imagen.FileName;
                await this.upload.UploadFileAsync(imagen, Folders.Libros);
                this.repo.AddLibro(nombre, numpag, fileName, urlcompra, descripcion, idioma, fecha_publicacion, idautor);
            }
            return RedirectToAction("Libros", "Administracion");
        }


        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        public IActionResult EditarLibro(int id)
        {
            ViewData["AUTORES"] = this.repo.GetAutores();
            return View(this.repo.GetDatosLibroDef(id));
        }


        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        [HttpPost]
        public async Task<IActionResult> EditarLibro(int id, string nombre, int numpag, IFormFile imagen, string urlcompra, string descripcion, string idioma, DateTime fecha_publicacion, int idautor, string nImagen)
        {
            if (imagen == null)
            {
                this.repo.UpdateLibro(id, nombre, numpag, nImagen, urlcompra, descripcion, idioma, fecha_publicacion, idautor);
            }
            else
            {
                string fileName = imagen.FileName;
                await this.upload.UploadFileAsync(imagen, Folders.Libros);
                this.repo.UpdateLibro(id, nombre, numpag, fileName, urlcompra, descripcion, idioma, fecha_publicacion, idautor);
            }
            return RedirectToAction("Libros", "Administracion");
        }


        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        public IActionResult Autores()
        {
            return View(this.repo.GetAutores());
        }

        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        [HttpPost]
        public void EliminarAutor(int id)
        {
            this.repo.DeleteAutor(id);
        }

        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        public IActionResult NuevoAutor()
        {
            return View();
        }

        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        [HttpPost]
        public async Task<IActionResult> NuevoAutor(string nombre, string nacionalidad, DateTime fechaNac, IFormFile imagen, string descripcion, int numLibros, string wiki)
        {
            if (imagen == null)
            {
                this.repo.AddAutor(nombre, nacionalidad, fechaNac, null, descripcion, numLibros, wiki);
            }
            else
            {
                string fileName = imagen.FileName;
                await this.upload.UploadFileAsync(imagen, Folders.Autores);
                this.repo.AddAutor(nombre, nacionalidad, fechaNac, fileName, descripcion, numLibros, wiki);
            }
            return RedirectToAction("Autores", "Administracion");
        }

        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        public IActionResult EditarAutor(int id)
        {
            return View(this.repo.GetDatosAutor(id));
        }


        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        [HttpPost]
        public async Task<IActionResult> EditarAutor(int id, string nombre, string nacionalidad, DateTime fechaNac, IFormFile imagen, string descripcion, int numLibros, string wiki, string nImagen)
        {
            if (imagen == null)
            {
                this.repo.UpdateAutor(id, nombre, nacionalidad, fechaNac, nImagen, descripcion, numLibros, wiki);
            }
            else
            {
                string fileName = imagen.FileName;
                await this.upload.UploadFileAsync(imagen, Folders.Autores);
                this.repo.UpdateAutor(id, nombre, nacionalidad, fechaNac, fileName, descripcion, numLibros, wiki);

            }
            return RedirectToAction("Autores", "Administracion");
        }


        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        public IActionResult LibrosBiblioteca(int id)
        {
            ViewData["LIBROSADD"] = this.repo.GetLibrosNotInBiblioteca(id);
            @ViewData["IDBIBLIO"] = id;
            return View(this.repo.GetLibrosBiblioteca(id));
        }

        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        [HttpPost]
        public void AddLibroBiblio(int idBiblio, int idLibro)
        {
            this.repo.AddLibroBiblio(idBiblio, idLibro);
        }

        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        [HttpPost]
        public void EliminarLibroBiblioteca(int idBiblio, int idLibro)
        {
            this.repo.DeleteLibroBiblio(idBiblio, idLibro);
        }


        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        public IActionResult PrestamosBiblioteca(int id)
        {
            @ViewData["IDBIBLIO"] = id;
            return View(this.repo.GetReservasBiblio(id));
        }


        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        [HttpPost]
        public void EliminarPrestamoBiblioteca(int id)
        {
            this.repo.DeleteReserva(id);
        }


        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        [HttpPost]
        public void RecogerLibro(int idPrestamo, int idBiblio)
        {
            this.repo.RecogerLibro(idPrestamo, idBiblio);
        }


        [AuthorizeUsers(Policy = "ADMIN")]
        [AuthorizeUsers]
        [HttpPost]
        public void DevolverLibro(int idPrestamo, int idBiblio)
        {
            this.repo.DevolverLibro(idPrestamo, idBiblio);
        }
    }
}
