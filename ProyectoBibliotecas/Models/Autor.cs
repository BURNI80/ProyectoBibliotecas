using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoBibliotecas.Models
{
    [Table("AUTOR")]
    public class Autor
    {
        [Key]
        [Column("ID_AUTOR")]
        public int ID_AUTOR { get; set; }

        [Column("NOMBRE")]
        public string NOMBRE { get; set; }

        [Column("NACIONALIDAD")]
        public string? NACIONALIDAD { get; set; }

        [Column("FECHA_NACIMIENTO")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FECHA_NACIMIENTO { get; set; }

        [Column("IMAGEN")]
        public string? IMAGEN { get; set; }

        [Column("HISTORIA")]
        public string? HISTORIA { get; set; }

        [Column("NUM_LIBROS")]
        public int? NUM_LIBROS { get; set; }

        [Column("WIKI")]
        public string? WIKI { get; set; }

    }
}
