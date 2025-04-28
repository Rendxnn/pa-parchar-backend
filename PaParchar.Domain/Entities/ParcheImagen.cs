using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaParchar.Domain.Entities
{
    [Table("parche_imagenes")]
    public class ParcheImagen
        : _BaseEntity<Guid>
    {
        [Required]
        [Column("parche_id")]
        public Guid ParcheId { get; set; }

        [Required]
        [StringLength(500)]
        [Column("imagen_url")]
        public string ImagenUrl { get; set; } = null!;

        [Column("descripcion")]
        [StringLength(255)]
        public string? Descripcion { get; set; }

        [Column("orden")]
        public int Orden { get; set; } = 0;

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [ForeignKey("ParcheId")]
        public virtual Parche? Parche { get; set; }
    }
} 