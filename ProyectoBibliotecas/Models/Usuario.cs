using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoBibliotecas.Models
{
    [Table("USUARIO")]
    public class Usuario
    {
        [Key]
        [Column("DNI_USUARIO")]
        public string DNI_USUARIO { get; set; }

        [Column("USUARIO")]
        public string USUARIO { get; set; }

        [Column("CONTRASEÑA")]
        public string CONTRASEÑA { get; set; }

        [Column("ROL")]
        public string ROL { get; set; }

        //[Required]
        [Column("NOMBRE")]
        public string NOMBRE { get; set; }

        [Column("APELLIDO")]
        public string APELLIDO { get; set; }

        [Column("EMAIL")]
        public string EMAIL { get; set; }

        [Column("TELEFONO")]
        public int TELEFONO { get; set; }

        [Column("SALT")]
        public string SALT { get; set; }

        [Column("PASSWORD")]
        public byte[] PASSWORD { get; set; }

    }
}
