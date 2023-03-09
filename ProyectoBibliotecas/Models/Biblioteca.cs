using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoBibliotecas.Models
{
    [Table("BIBLIOTECA")]
    public class Biblioteca
    {
        [Key]
        [Column("ID_BIBLIOTECA")]
        public int ID_BIBLIOTECA { get; set; }

        [Column("NOMBRE")]
        public string NOMBRE { get; set; }

        [Column("DIRECCION")]
        public string DIRECCION { get; set; }

        [Column("TELEFONO")]
        public int? TELEFONO { get; set; }

        [Column("WEB")]
        public string? WEB { get; set; }

        [Column("HORA_APERTURA")]
        public TimeSpan HORA_APERTURA { get; set; }

        [Column("HORA_CIERRE")]
        public TimeSpan HORA_CIERRE { get; set; }

        [Column("IMAGEN")]
        public string? IMAGEN { get; set; }
    }
}
