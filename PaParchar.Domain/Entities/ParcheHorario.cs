using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaParchar.Domain.Entities
{
    [Table("parches_horarios")]
    public class ParcheHorario
        : _BaseEntity<Guid>
    {
        [Required]
        [Column("parche_id")]
        public Guid ParcheId { get; set; }

        [Required]
        [StringLength(1)]
        [Column("dia")]
        public string Dia { get; set; } = null!;

        [Required]
        [Column("hora_inicio")]
        public TimeOnly HoraInicio { get; set; }

        [Required]
        [Column("hora_fin")]
        public TimeOnly HoraFin { get; set; }

        [ForeignKey("ParcheId")]
        public virtual Parche Parche { get; set; } = null!;
    }
} 