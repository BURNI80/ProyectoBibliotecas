using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoBibliotecas.Models
{
    [Table("COMENTARIO")]
    public class ComentarioBasico
    {
        [Key]
        [Column("ID_COMENTARIO")]
        public int ID_COMENTARIO { get; set; }

        [Column("ID_LIBRO")]
        public int ID_LIBRO { get; set; }

        [Column("DNI_USUARIO")]
        public string DNI_USUARIO { get; set; }

        [Column("FECHA_COMENTARIO")]
        public DateTime FECHA_COMENTARIO { get; set; }

        [Column("MEGUSTAS")]
        public int MEGUSTAS { get; set; }

        [Column("MENSAJE")]
        public string MENSAJE { get; set; }
    }
}
