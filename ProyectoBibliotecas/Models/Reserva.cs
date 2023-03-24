using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoBibliotecas.Models
{
    [Table("PRESTAMO")]
    public class Reserva
    {
        [Key]
        [Column("ID_PRESTAMO")]
        public int ID_PRESTAMO { get; set; }

        [Column("DNI_USUARIO")]
        public string DNI_USUARIO { get; set; }

        [Column("ID_LIBRO")]
        public int ID_LIBRO { get; set; }

        [Column("ID_BIBLIOTECA")]
        public int ID_BIBLIOTECA { get; set; }

        [Column("FECHA_INICIO")]
        public DateTime FECHA_INICIO { get; set; }

        [Column("FECHA_FIN")]
        public DateTime FECHA_FIN { get; set; }

        [Column("DEVUELTO")]
        public bool DEVUELTO { get; set; }

        [Column("COMPLETADO")]
        public bool COMPLETADO { get; set; }
    }
}
