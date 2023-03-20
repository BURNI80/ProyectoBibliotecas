﻿using Microsoft.EntityFrameworkCore;
using ProyectoBibliotecas.Models;

namespace ProyectoBibliotecas.Data
{
    public class BibliotecasContext :DbContext
    {

        public BibliotecasContext(DbContextOptions<BibliotecasContext> options) : base(options) { }

        public DbSet<Biblioteca> Bibliotecas { get; set; }

        public DbSet<Libro> Libros { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<LibroDisponibilidad> LibroDisponibilidad { get; set; }

        public DbSet<Valoracion> Valoraciones { get; set; }

        public DbSet<Comentario> Comentarios { get; set; }

        public DbSet<ComentarioID> ComentariosID { get; set; }

        public DbSet<ComentarioBasico> ComentariosBasico { get; set; }

        public DbSet<Autor> Autores { get; set; }

        public DbSet<DeseosLeido> ListaDeseos { get; set; }

        public DbSet<LibroDeseo> LibrosDeseo { get; set; }

        public DbSet<ReservaUsuario> ReservasUsuario { get; set; }

        public DbSet<Share> Share { get; set; }






    }
}
