﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoBibliotecas.Helpers;
using ProyectoBibliotecas.Data;
using ProyectoBibliotecas.Models;
using System.Data;
using System.Data.Common;
using System.Net;
using Newtonsoft.Json.Linq;

namespace ProyectoBibliotecas.Repositorys
{
    public class BibliotecasRepository
    {
        private BibliotecasContext context;

        public BibliotecasRepository(BibliotecasContext context)
        {
            this.context = context;
        }


        public async Task Register(string nombre, string apellidos, string dni, string usuario, string password, string email, int telefono)
        {
            Usuario user = new Usuario();
            user.NOMBRE = nombre;
            user.APELLIDO = apellidos;
            user.DNI_USUARIO = dni;
            user.USUARIO = usuario;
            user.CONTRASEÑA = password;
            user.EMAIL = email;
            user.TELEFONO = telefono;
            user.ROL = "USUARIO";
            user.SALT = HelperCryptography.GenerateSalt();
            user.PASSWORD = HelperCryptography.EncryptPassword(password, user.SALT);

            this.context.Usuarios.Add(user);
            await this.context.SaveChangesAsync();
        }

        public Usuario Login(string dni, string pass)
        {
            Usuario usu = this.context.Usuarios.FirstOrDefault(z => z.DNI_USUARIO == dni);
            if (usu == null)
            {
                return null;
            }
            else
            {
                byte[] passBBDD = usu.PASSWORD;
                byte[] passInput = HelperCryptography.EncryptPassword(pass, usu.SALT);
                bool res = HelperCryptography.ComapreArrays(passBBDD, passInput);
                if (res == true)
                {
                    return usu;
                }
                else
                {
                    return null;
                }
            }
        }


        public List<Biblioteca> GetBibliotecas()
        {
            return this.context.Bibliotecas.ToList();
        }

        public List<Biblioteca> SearchBiblioteca(string input)
        {
            string sql = "SP_BUSCARBIBLIOTECAS @INPUT";
            SqlParameter p = new SqlParameter("@INPUT", input);
            var consulta = this.context.Bibliotecas.FromSqlRaw(sql, p);
            return consulta.AsEnumerable().ToList();
        }

        public Biblioteca GetDatosBiblioteca(int id)
        {
            var consulta = from data in this.context.Bibliotecas
                           where data.ID_BIBLIOTECA == id
                           select data;
            return consulta.FirstOrDefault();
        }

        public List<LibroDefault> GetLibrosTodos()
        {
            return this.context.LibrosDef.ToList();
        }


        public List<LibroDisponibilidad> GetLibrosBiblioteca(int id)
        {
            string sql = "SP_BUSCARLIBRO @ID_BIBLIOTECA";
            SqlParameter p = new SqlParameter("@ID_BIBLIOTECA", id);
            var consulta = this.context.LibroDisponibilidad.FromSqlRaw(sql, p);
            return consulta.AsEnumerable().ToList();
        }

        public List<LibroDisponibilidad> SearchLibroBiblioteca(int id, string input, char option)
        {
            string sql = "";
            if (input == null)
            {
                return GetLibrosBiblioteca(id);
            }
            if (option == 'T')
            {
                sql = "SP_BUSCARLIBRONOMBRE @INPUT, @ID_BIBLIOTECA";
            }
            else if (option == 'A')
            {
                sql = "SP_BUSCARLIBROAUTOR @INPUT, @ID_BIBLIOTECA";
            }
            SqlParameter p1 = new SqlParameter("@INPUT", input);
            SqlParameter p2 = new SqlParameter("@ID_BIBLIOTECA", id);
            var consulta = this.context.LibroDisponibilidad.FromSqlRaw(sql, p1, p2);
            return consulta.AsEnumerable().ToList();
        }

        public Libro GetDatosLibro(int id)
        {
            string sql = "SP_DETALLESLIBRO @ID_LIBRO";
            SqlParameter p1 = new SqlParameter("@ID_LIBRO", id);
            var consulta = this.context.Libros.FromSqlRaw(sql, p1);
            return consulta.AsEnumerable().First();
        }

        public LibroDefault GetDatosLibroDef(int id)
        {
            return this.context.LibrosDef.Where(x => x.ID_LIBRO == id).FirstOrDefault();
        }

        public int GetValoraciones(int id)
        {
            var consulta = from data in this.context.Valoraciones.AsEnumerable()
                           where data.ID_LIBRO == id
                           select data;
            int cuenta = consulta.ToList().Count();
            return cuenta;
        }

        public List<Comentario> GetComentarios(int id, string dni)
        {
            string sql = "SP_GETCOMENTARIOSLIBRO @ID_LIBRO ,@DNI_USUARIO";
            SqlParameter p1 = new SqlParameter("@ID_LIBRO", id);
            SqlParameter p2 = new SqlParameter("@DNI_USUARIO", dni ?? (object)DBNull.Value);
            var consulta = this.context.Comentarios.FromSqlRaw(sql, p1, p2);
            return consulta.AsEnumerable().ToList();
        }

        public void LikeComentario(int orden, int idComentario, string dni)
        {
            string sql = "SP_LIKECOMENTARIO @ID_COMENTARIO, @IDENT,@DNI_USUARIO";
            SqlParameter p1 = new SqlParameter("@ID_COMENTARIO", idComentario);
            SqlParameter p2 = new SqlParameter("@IDENT", orden);
            SqlParameter p3 = new SqlParameter("@DNI_USUARIO", dni);
            int rowsAffected = this.context.Database.ExecuteSqlRaw(sql, p1, p2, p3);
        }

        public List<Autor> GetAutores()
        {
            return this.context.Autores.ToList();
        }

        public List<Autor> SearchAutor(string input)
        {
            string sql = "SP_BUSCARAUTOR @INPUT";
            SqlParameter p = new SqlParameter("@INPUT", input);
            var consulta = this.context.Autores.FromSqlRaw(sql, p);
            return consulta.AsEnumerable().ToList();
        }

        public void PostComentario(int idLibro, string dni, DateTime fecha, string textoComentario, int rating)
        {
            string sql = "SP_CREATECOMENTARIORESENIA @ID_LIBRO, @DNI_USUARIO, @FECHA_COMENTARIO, @MENSAJE, @PUNTUACION";
            SqlParameter p1 = new SqlParameter("@ID_LIBRO", idLibro);
            SqlParameter p2 = new SqlParameter("@DNI_USUARIO", dni);
            SqlParameter p3 = new SqlParameter("@FECHA_COMENTARIO", fecha);
            SqlParameter p4 = new SqlParameter("@MENSAJE", textoComentario ?? (object)DBNull.Value);
            SqlParameter p5 = new SqlParameter("@PUNTUACION", rating);
            int rowsAffected = this.context.Database.ExecuteSqlRaw(sql, p1, p2, p3, p4, p5);
        }

        public int LibroDeseo(int idLibro, string dni)
        {
            var consulta = from data in this.context.ListaDeseos
                           where data.ID_LIBRO == idLibro && data.DNI_USUARIO.Equals(dni)
                           select data.LEIDO;

            if (!consulta.Any())
            {
                return -1;
            }
            else
            {
                return consulta.FirstOrDefault();
            }
        }

        public void AddListaLibro(string dni, int idLibro, int orden)
        {
            DateTime fecha = DateTime.Now;
            string sql = "SP_LISTADESEOS @ORDEN , @DNI_USUARIO, @ID_LIBRO, @FECHA";
            SqlParameter p1 = new SqlParameter("@ORDEN", orden);
            SqlParameter p2 = new SqlParameter("@DNI_USUARIO", dni);
            SqlParameter p3 = new SqlParameter("@ID_LIBRO", idLibro);
            SqlParameter p4 = new SqlParameter("@FECHA", fecha);
            int rowsAffected = this.context.Database.ExecuteSqlRaw(sql, p1, p2, p3, p4);
        }


        public int NComentarios(string id)
        {
            return this.context.ComentariosBasico.Where(x => x.DNI_USUARIO.Equals(id)).Count();
        }

        public int NReseñas(string id)
        {
            return this.context.Valoraciones.Where(x => x.DNI_USUARIO.Equals(id)).Count();
        }

        public int NLibrosLeidos(string id)
        {
            return this.context.ListaDeseos.Where(x => x.DNI_USUARIO.Equals(id) && x.LEIDO == 1).Count();
        }

        public List<ReservaUsuario> GetReservasUsuario(string id)
        {
            string sql = "SP_PRESTAMOSUSUARIO @DNI_USUARIO";
            SqlParameter p1 = new SqlParameter("@DNI_USUARIO", id);
            var consulta = this.context.ReservasUsuario.FromSqlRaw(sql, p1);
            return consulta.AsEnumerable().ToList();
        }

        public List<ComentarioBasico> GetComentariosUsuario(string id)
        {
            return this.context.ComentariosBasico.Where(x => x.DNI_USUARIO.Equals(id)).ToList();
        }

        public void DeleteComentario(int id)
        {
            string sql = "SP_DELETECOMENTARIO @ID_COMENTARIO";
            SqlParameter p1 = new SqlParameter("@ID_COMENTARIO", id);
            int rowsAffected = this.context.Database.ExecuteSqlRaw(sql, p1);
        }

        public List<LibroDeseo> GetFavoritos(string id)
        {
            string sql = "SP_LIBROSLISTA @DNI_USUARIO";
            SqlParameter p1 = new SqlParameter("@DNI_USUARIO", id);
            var consulta = this.context.LibrosDeseo.FromSqlRaw(sql, p1);
            return consulta.AsEnumerable().ToList();
        }

        public string GenerateToken()
        {
            var token = Guid.NewGuid().ToString("N") + new Random().Next(1000, 9999).ToString();
            return token;
        }

        public Compartido GetToken(string dni, string token)
        {
            string sql = "SP_COMPARTIR @DNI_USUARIO , @TOKEN";
            SqlParameter p1 = new SqlParameter("@DNI_USUARIO", dni);
            SqlParameter p2 = new SqlParameter("@TOKEN", token);
            return this.context.Share.FromSqlRaw(sql, p1, p2).AsEnumerable().FirstOrDefault();
        }

        public Compartido GetShare(string id)
        {
            return this.context.Share.Where(x => x.DNI_USUARIO.Equals(id)).FirstOrDefault();
        }

        public Usuario GetUsuario(string dni)
        {
            return this.context.Usuarios.Where(x => x.DNI_USUARIO.Equals(dni)).FirstOrDefault();
        }

        public Autor GetDatosAutor(int id)
        {
            return this.context.Autores.Where(x => x.ID_AUTOR == id).FirstOrDefault();
        }

        public List<LibroDefault> GetLibrosAutor(int id)
        {
            return this.context.LibrosDef.Where(x => x.ID_AUTOR == id).ToList();
        }

        public List<LibroDefault> SearchLibroAutor(int id, string input)
        {
            string sql = "SP_SEARCHLIBRODEAUTOR @ID_AUTOR, @INPUT";
            SqlParameter p1 = new SqlParameter("@ID_AUTOR", id);
            SqlParameter p2 = new SqlParameter("@INPUT", input);
            var consulta = this.context.LibrosDef.FromSqlRaw(sql, p1,p2);
            return consulta.ToList();
        }

        public void UpdateUsuario(string id, string nombre, string apellido, string email, int telefono, string usuario)
        {
            Usuario user = this.context.Usuarios.Where(x => x.DNI_USUARIO.Equals(id)).FirstOrDefault();
            user.NOMBRE = nombre;
            user.APELLIDO = apellido;
            user.EMAIL = email;
            user.TELEFONO = telefono;
            user.USUARIO = usuario;
            this.context.SaveChangesAsync();
        }


        public List<BibliotecaSimple> GetLibroDisponible(int id)
        {
            string sql = "SP_LIBROBIBLIOTECADISPONIBLE @ID_LIBRO";
            SqlParameter p1 = new SqlParameter("@ID_LIBRO", id);
            List<BibliotecaSimple> consulta = this.context.BibliotecasSimples.FromSqlRaw(sql, p1).ToList();
            return consulta;
        }

        public List<Reserva> GetResrevasLibro(int id , int idBiblio)
        {
            return this.context.Reservas.Where(x => x.ID_LIBRO == id && x.ID_BIBLIOTECA == idBiblio).ToList();
        }

        public List<string> GetDaysBetween(DateTime fecha_inicio, DateTime fecha_fin)
        {
            var days = new List<string>();
            for (DateTime date = fecha_inicio; date <= fecha_fin; date = date.AddDays(1))
            {
                days.Add(date.ToString("dd/MM/yyyy"));
            }
            return days;
        }

        public void CreateReserva(string dni, int idLibro, int idBiblio, DateTime fechaI, DateTime fechaF)
        {
            int nuevoId = this.context.Reservas.Any() ? this.context.Reservas.Max(x => x.ID_PRESTAMO) + 1 : 1;
            Reserva r = new Reserva();
            r.ID_PRESTAMO = nuevoId;
            r.DNI_USUARIO = dni;
            r.ID_LIBRO = idLibro;
            r.ID_BIBLIOTECA = idBiblio;
            r.FECHA_INICIO = fechaI;
            r.FECHA_FIN = fechaF;
            r.DEVUELTO = true;
            r.COMPLETADO = false;
            this.context.Reservas.Add(r);
            this.context.SaveChangesAsync();
        }

        public void DeleteReserva(int id)
        {
            Reserva r = this.context.Reservas.Where(x => x.ID_PRESTAMO == id).FirstOrDefault();
            this.context.Reservas.Remove(r);
            this.context.SaveChangesAsync();
        }

        public void DeleteBiblioteca(int id)
        {
            Biblioteca b = this.context.Bibliotecas.Where(x => x.ID_BIBLIOTECA == id).FirstOrDefault();
            this.context.Bibliotecas.Remove(b);
            this.context.SaveChangesAsync();
        }

        public void AddBiblio(string nombre, string direccion, int telefono, string web, TimeSpan hora_apertura, TimeSpan hora_cierre, string imagen)
        {
            int nuevoId = this.context.Bibliotecas.Any() ? this.context.Bibliotecas.Max(x => x.ID_BIBLIOTECA) + 1 : 1;
            Biblioteca b = new Biblioteca();
            b.ID_BIBLIOTECA = nuevoId;
            b.NOMBRE = nombre;
            b.DIRECCION = direccion;
            b.TELEFONO = telefono;
            b.WEB = web;
            b.IMAGEN = imagen;
            b.HORA_APERTURA = hora_apertura;
            b.HORA_CIERRE = hora_cierre;
            this.context.Bibliotecas.Add(b);
            this.context.SaveChangesAsync();
        }

        public void UpdateBiblio(int id, string nombre, string direccion, int telefono, string web, TimeSpan hora_apertura, TimeSpan hora_cierre, string imagen)
        {
            Biblioteca b = GetDatosBiblioteca(id);
            b.NOMBRE = nombre;
            b.DIRECCION = direccion;
            b.TELEFONO = telefono;
            b.WEB = web;
            b.IMAGEN = imagen;
            b.HORA_APERTURA = hora_apertura;
            b.HORA_CIERRE = hora_cierre;
            this.context.SaveChangesAsync();
        }





        public void DeleteLibro(int id)
        {
            LibroDefault l = this.context.LibrosDef.Where(x => x.ID_LIBRO == id).FirstOrDefault();
            this.context.LibrosDef.Remove(l);
            this.context.SaveChangesAsync();
        }
        public void AddLibro(string nombre, int numpag, string imagen, string urlcompra, string descripcion, string idioma, DateTime fecha_publicacion, int idautor)
        {
            int nuevoId = this.context.LibrosDef.Any() ? this.context.LibrosDef.Max(x => x.ID_LIBRO) + 1 : 1;
            LibroDefault b = new LibroDefault();
            b.ID_LIBRO = nuevoId;
            b.NOMBRE = nombre;
            b.NUM_PAGINAS = numpag;
            b.IMAGEN = null;
            b.URL_COMPRA = urlcompra;
            b.DESCRIPCION = descripcion;
            b.IDIOMA = idioma;
            b.IMAGEN = imagen;
            b.FECHA_PUBLICACION = fecha_publicacion;
            b.ID_AUTOR = idautor;
            this.context.LibrosDef.Add(b);
            this.context.SaveChangesAsync();
        }
        public void UpdateLibro(int id, string nombre, int numpag, string imagen, string urlcompra, string descripcion, string idioma, DateTime fecha_publicacion, int idautor)
        {
            LibroDefault b = GetDatosLibroDef(id);
            b.NOMBRE = nombre;
            b.NUM_PAGINAS = numpag;
            b.IMAGEN = null;
            b.URL_COMPRA = urlcompra;
            b.DESCRIPCION = descripcion;
            b.IMAGEN = imagen;
            b.IDIOMA = idioma;
            b.FECHA_PUBLICACION = fecha_publicacion;
            b.ID_AUTOR = idautor;
            this.context.SaveChangesAsync();
        }



        public void DeleteAutor(int id)
        {
            Autor l = this.context.Autores.Where(x => x.ID_AUTOR == id).FirstOrDefault();
            this.context.Autores.Remove(l);
            this.context.SaveChangesAsync();
        }
        public void AddAutor(string nombre, string nacionalidad, DateTime fechaNac, string imagen, string descripcion, int numLibros, string wiki)
        {
            int nuevoId = this.context.Autores.Any() ? this.context.Autores.Max(x => x.ID_AUTOR) + 1 : 1;
            Autor b = new Autor();
            b.ID_AUTOR = nuevoId;
            b.NOMBRE = nombre;
            b.NACIONALIDAD = nacionalidad;
            b.FECHA_NACIMIENTO = fechaNac;
            b.IMAGEN = imagen;
            b.HISTORIA = descripcion;
            b.NUM_LIBROS = numLibros;
            b.WIKI = wiki;
            this.context.Autores.Add(b);
            this.context.SaveChangesAsync();
        }
        public void UpdateAutor(int id, string nombre, string nacionalidad, DateTime fechaNac, string imagen, string descripcion, int numLibros, string wiki)
        {
            Autor b = GetDatosAutor(id);
            b.NOMBRE = nombre;
            b.NACIONALIDAD = nacionalidad;
            b.FECHA_NACIMIENTO = fechaNac;
            b.IMAGEN = imagen;
            b.HISTORIA = descripcion;
            b.NUM_LIBROS = numLibros;
            b.WIKI = wiki;
            this.context.SaveChangesAsync();
        }

        public List<BibliotecaSimple> GetBibliotecasEditables(string id)
        {
            var consulta = from editorBiblioteca in context.EditorBiblioteca
                           join biblioteca in context.Bibliotecas
                           on editorBiblioteca.ID_BIBLIOTECA equals biblioteca.ID_BIBLIOTECA
                           where editorBiblioteca.DNI_USUARIO == id
                           select new
                           {
                               biblioteca.ID_BIBLIOTECA,
                               biblioteca.NOMBRE,
                           };
            List<BibliotecaSimple> biblios = consulta.ToList().ConvertAll(x => new BibliotecaSimple
            {
                ID_BIBLIOTECA = x.ID_BIBLIOTECA,
                NOMBRE = x.NOMBRE,
            });
            return biblios;
        }

        public List<BibliotecaSimple> GetBibliotecasSimples()
        {
            List<Biblioteca> biblios = this.context.Bibliotecas.ToList();
            List<BibliotecaSimple> bibliotecasSimples = biblios.Select(b =>
                    new BibliotecaSimple
                    {
                        ID_BIBLIOTECA = b.ID_BIBLIOTECA,
                        NOMBRE = b.NOMBRE
                    }).ToList();
            return bibliotecasSimples;
        }

        public List<LibroDefault> GetLibrosNotInBiblioteca(int id)
        {
            var consulta = from libro in this.context.LibrosDef
                           where !(from lb in this.context.LibrosBiblio
                                   where lb.ID_BIBLIOTECA == id
                                   select lb.ID_LIBRO)
                                   .Contains(libro.ID_LIBRO)
                           select libro;

            return consulta.ToList();
        }

        public void AddLibroBiblio(int idBiblio, int idLibro)
        {
            string sql = "SP_ADDLIBROBIBLIOTECA @ID_LIBRO ,@ID_BIBLIOTECA";
            SqlParameter p1 = new SqlParameter("@ID_LIBRO", idLibro);
            SqlParameter p2 = new SqlParameter("@ID_BIBLIOTECA", idBiblio);
            int rowsAffected = this.context.Database.ExecuteSqlRaw(sql, p1, p2);
        }

        public void DeleteLibroBiblio(int idBiblio, int idLibro)
        {
            string sql = "SP_DELETELIBROBIBLIOTECA @ID_LIBRO ,@ID_BIBLIOTECA";
            SqlParameter p1 = new SqlParameter("@ID_LIBRO", idLibro);
            SqlParameter p2 = new SqlParameter("@ID_BIBLIOTECA", idBiblio);
            int rowsAffected = this.context.Database.ExecuteSqlRaw(sql, p1, p2);
        }

        public List<ReservaNLibro> GetReservasBiblio(int id)
        {
            string sql = "SP_GETPRESTAMOBIBLIOTECA @ID_BIBLIOTECA";
            SqlParameter p1 = new SqlParameter("@ID_BIBLIOTECA", id);
            List<ReservaNLibro> consulta = this.context.ReservaNLibros.FromSqlRaw(sql, p1).ToList();
            return consulta;
        }

        public void RecogerLibro(int idPrestamo,int idBiblio)
        {
            string sql = "SP_PRESTARLIBRO @ID_PRESTAMO ,@ID_BIBLIOTECA, @ID_LIBRO";
            SqlParameter p1 = new SqlParameter("@ID_PRESTAMO", idPrestamo);
            SqlParameter p2 = new SqlParameter("@ID_BIBLIOTECA", idBiblio);
            Reserva r = this.context.Reservas.Where(x => x.ID_PRESTAMO == idPrestamo).FirstOrDefault();
            int idLibro = r.ID_LIBRO;
            SqlParameter p3 = new SqlParameter("@ID_LIBRO", idLibro);
            int rowsAffected = this.context.Database.ExecuteSqlRaw(sql, p1, p2,p3);
        }

        public void DevolverLibro(int idPrestamo, int idBiblio)
        {
            string sql = "SP_RECIVIRLIBRO @ID_PRESTAMO ,@ID_BIBLIOTECA, @ID_LIBRO";
            SqlParameter p1 = new SqlParameter("@ID_PRESTAMO", idPrestamo);
            SqlParameter p2 = new SqlParameter("@ID_BIBLIOTECA", idBiblio);
            Reserva r = this.context.Reservas.Where(x => x.ID_PRESTAMO == idPrestamo).FirstOrDefault();
            int idLibro = r.ID_LIBRO;
            SqlParameter p3 = new SqlParameter("@ID_LIBRO", idLibro);
            int rowsAffected = this.context.Database.ExecuteSqlRaw(sql, p1, p2, p3);
        }
    }
}
