using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoBibliotecas.Models
{
    [Keyless]
    public class Share
    {
        [Column("DNI_USUARIO")]
        public string DNI_USUARIO { get; set; }

        [Column("TOKEN")]
        public string TOKEN { get; set; }

    }
}
