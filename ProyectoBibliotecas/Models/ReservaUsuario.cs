using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoBibliotecas.Models
{
    public class ReservaUsuario
    {
        [Key]
        [Column("ID_PRESTAMO")]
        public int ID_PRESTAMO { get; set; }

        [Column("NLIBRO")]
        public string NLIBRO { get; set; }

        [Column("NBIBLIOTECA")]
        public string NBIBLIOTECA { get; set; }

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
