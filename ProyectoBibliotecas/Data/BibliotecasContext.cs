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




    }
}