using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoBibliotecas.Data;
using ProyectoBibliotecas.Models;
using System.Data;
using System.Data.Common;

namespace ProyectoBibliotecas.Repositorys
{
    public class BibliotecasRepository
    {
        private BibliotecasContext context;

        public BibliotecasRepository(BibliotecasContext context)
        {
            this.context = context;
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
            return (Biblioteca)consulta.First();
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
            string sql = "SP_DETALLELIBRO @ID_LIBRO";
            SqlParameter p1 = new SqlParameter("@ID_LIBRO", id);
            var consulta = this.context.Libros.FromSqlRaw(sql, p1);
            return consulta.AsEnumerable().First();
        }

        public int GetValoraciones(int id)
        {
            var consulta = from data in this.context.Valoraciones.AsEnumerable()
                           where data.ID_LIBRO == id
                           select data;

            return consulta.ToList().Count();
        }

        public List<Comentario> GetComentarios(int id)
        {
            var consulta = from data in this.context.Comentarios.AsEnumerable()
                           where data.ID_LIBRO == id
                           select data;
            return consulta.ToList();
        }


    }
}
