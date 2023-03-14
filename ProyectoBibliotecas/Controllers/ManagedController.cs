using Microsoft.AspNetCore.Mvc;
using ProyectoBibliotecas.Extensions;
using ProyectoBibliotecas.Models;
using ProyectoBibliotecas.Repositorys;

namespace ProyectoBibliotecas.Controllers
{
    public class ManagedController : Controller
    {
        BibliotecasRepository repo;

        public ManagedController(BibliotecasRepository repo)
        {
            this.repo = repo;
        }


        public IActionResult Login()
        {
            return View();
        }

        //REGISTRO
        [HttpPost]
        public async Task<IActionResult> Login(string nombre, string apellidos, string dni, string usuario, string password, string email, int telefono)
        {
            if (usuario == null)
            {
                //LOGIN
                Usuario user = this.repo.Login(dni, password);
                if (user == null)
                {
                    ViewData["MSG"] = "Incorrecto";
                    return View();
                }
                else
                {
                    HttpContext.Session.SetObject("user", user);
                    return RedirectToAction("IndexBibliotecas","Bibliotecas");
                }
            }
            else
            {
                await this.repo.Register(nombre, apellidos, dni, usuario, password, email, telefono);
            }
            return View();

        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Remove("user");
            return RedirectToAction("Login");
        }
    }
}
