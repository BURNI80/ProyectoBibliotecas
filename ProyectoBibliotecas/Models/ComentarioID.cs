using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoBibliotecas.Models
{
    public class ComentarioID
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

        [Column("COMENTARIO_LIKE")]
        public int? COMENTARIO_LIKE { get; set; }
    }
}