using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoBibliotecas.Models
{
    [Table("LIBRO_BIBLIOTECA")]
    [Keyless]
    public class LibroBiblio
    {
        [Column("ID_LIBRO")]
        public int ID_LIBRO { get; set; }

        [Column("ID_BIBLIOTECA")]
        public int ID_BIBLIOTECA { get; set; }

        [Column("DISPONIBLE")]
        public bool DISPONIBLE { get; set; }

    }
}
