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

        public List<Libro> GetLibrosBiblioteca(int id)
        {
            string sql = "SP_BUSCARLIBRO @ID_BIBLIOTECA";
            SqlParameter p = new SqlParameter("@ID_BIBLIOTECA", id);
            var consulta = this.context.Libros.FromSqlRaw(sql, p);
            return consulta.AsEnumerable().ToList();
        }


    }
}
