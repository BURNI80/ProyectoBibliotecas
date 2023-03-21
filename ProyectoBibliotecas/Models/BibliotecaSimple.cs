using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoBibliotecas.Models
{
    [Keyless]
    public class BibliotecaSimple
    {
        [Column("ID_BIBLIOTECA")]
        public int ID_BIBLIOTECA { get; set; }

        [Column("NOMBRE")]
        public string NOMBRE { get; set; }

    }
}
