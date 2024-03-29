﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoBibliotecas.Models
{
    public class Libro
    {
        [Key]
        [Column("ID_LIBRO")]
        public int ID_LIBRO { get; set; }

        [Column("NOMBRE_LIBRO")]
        public string NOMBRE_LIBRO { get; set; }

        [Column("NUM_PAGINAS")]
        public int NUM_PAGINAS { get; set; }

        [Column("VALORACION_MEDIA")]
        public double? VALORACION_MEDIA { get; set; }

        [Column("IMAGEN")]
        public string? IMAGEN { get; set; }

        [Column("URL_COMPRA")]
        public string? URL_COMPRA { get; set; }

        [Column("DESCRIPCION")]
        public string? DESCRIPCION { get; set; }

        [Column("IDIOMA")]
        public string? IDIOMA { get; set; }

        [Column("FECHA_PUBLICACION")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FECHA_PUBLICACION { get; set; }

        [Column("NOMBRE_AUTOR")]
        public string NOMBRE_AUTOR { get; set; }

    }
}
