using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProyectoBibliotecas.Models
{
    [Table("DESEOS_LEIDO")]
    [Keyless]
    public class DeseosLeido
    {
        [Column("DNI_USUARIO")]
        public string DNI_USUARIO { get; set; }

        [Column("ID_LIBRO")]
        public int ID_LIBRO { get; set; }

        [Column("LEIDO")]
        public int LEIDO { get; set; }

        [Column("FECHA_LEIDO")]
        public DateTime? FECHA_LEIDO { get; set; }

    }
}
