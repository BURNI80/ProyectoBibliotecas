using Microsoft.Data.SqlClient;
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

        public Share GetToken(string dni, string token)
        {
            string sql = "SP_SHARE @DNI_USUARIO , @TOKEN";
            SqlParameter p1 = new SqlParameter("@DNI_USUARIO", dni);
            SqlParameter p2 = new SqlParameter("@TOKEN", token);
            return this.context.Share.FromSqlRaw(sql, p1,p2).AsEnumerable().FirstOrDefault();
        }

        public Share GetShare(string id)
        {
            return this.context.Share.Where(x => x.DNI_USUARIO.Equals(id)).FirstOrDefault();
        }

        public Usuario GetUsuario(string dni)
        {
            return this.context.Usuarios.Where(x => x.DNI_USUARIO.Equals(dni)).FirstOrDefault();
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


        public string[] GetLibroDisponible(int id)
        {
            string sql = "SP_LIBROBIBLIOTECADISPONIBLE @ID_LIBRO";
            SqlParameter p1 = new SqlParameter("@ID_LIBRO", id);
            string consulta = this.context.Database.ExecuteSqlRaw(sql, p1).ToString();
            return consulta;
        }
    }
}
