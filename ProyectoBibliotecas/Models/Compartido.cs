using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoBibliotecas.Models
{
    [Table("COMPARTIR")]
    [Keyless]
    public class Compartido
    {
        [Column("DNI_USUARIO")]
        public string DNI_USUARIO { get; set; }

        [Column("TOKEN")]
        public string TOKEN { get; set; }

    }
}
