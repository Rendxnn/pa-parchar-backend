using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PaParchar.Domain.Entities
{
   [Table("usuarios")]
    public class Usuario
        : _BaseEntity<Guid>
    {

        [Required]
        [StringLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; } = null!;

        [Required]
        [StringLength(100)]
        [Column("apellido")]
        public string Apellido { get; set; } = null!;

        [Required]
        [StringLength(150)]
        [Column("email")]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(50)]
        [Column("telefono")]
        public string Telefono { get; set; } = null!;

        [Required]
        [Column("fecha_nacimiento")]
        public DateTime FechaNacimiento { get; set; }
    }
}
