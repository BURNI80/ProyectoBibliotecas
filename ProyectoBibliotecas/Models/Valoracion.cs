using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoBibliotecas.Models
{
    [Table("VALORACION_LIBRO")]
    public class Valoracion
    {
        [Key]
        [Column("ID_VALORACION")]
        public int ID_VALORACION { get; set; }

        [Column("ID_LIBRO")]
        public int ID_LIBRO { get; set; }

        [Column("DNI_USUARIO")]
        public string DNI_USUARIO { get; set; }

        [Column("FECHA")]
        public DateTime FECHA { get; set; }

        [Column("PUNTUACION")]
        public double PUNTUACION { get; set; }
    }
}
