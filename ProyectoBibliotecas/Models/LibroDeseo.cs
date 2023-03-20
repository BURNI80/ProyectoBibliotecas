using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoBibliotecas.Models
{
    public class LibroDeseo
    {
        [Key]
        [Column("ID_LIBRO")]
        public int ID_LIBRO { get; set; }

        [Column("NOMBRE_LIBRO")]
        public string NOMBRE_LIBRO { get; set; }

        [Column("IMAGEN")]
        public string? IMAGEN { get; set; }

        [Column("FECHA_LEIDO")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FECHA_LEIDO { get; set; }

        [Column("NOMBRE_AUTOR")]
        public string NOMBRE_AUTOR { get; set; }
    }
}
