using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoBibliotecas.Models
{
    [Table("EDITOR_BIBLIOTECA")]
    [Keyless]
    public class EditorBiblioteca
    {
        [Column("DNI_USUARIO")]
        public string DNI_USUARIO { get; set; }

        [Column("ID_BIBLIOTECA")]
        public int ID_BIBLIOTECA { get; set; }
    }
}
