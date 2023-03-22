using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoBibliotecas.Models
{
    public class LibroDisponibilidad
    {
        [Key]
        [Column("ID_LIBRO")]
        public int ID_LIBRO { get; set; }

        [Column("IMAGEN")]
        public string? IMAGEN { get; set; }

        [Column("NOMBRE_LIBRO")]
        public string NOMBRE_LIBRO { get; set; }

        [Column("NOMBRE_AUTOR")]
        public string NOMBRE_AUTOR { get; set; }

        [Column("NUM_PAGINAS")]
        public int? NUM_PAGINAS { get; set; }

        [Column("VALORACION_MEDIA")]
        public double? VALORACION_MEDIA { get; set; }

        [Column("DISPONIBLE")]
        public bool? DISPONIBLE { get; set; }
    }
}
