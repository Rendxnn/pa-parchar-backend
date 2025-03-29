using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaParchar.Domain.Entities
{
    [Table("parches_horarios")]
    public class ParcheHorario
        : _BaseEntity<Guid>
    {
        [Key]
        [Column("horario_id")]
        public Guid HorarioId { get; set; }

        [Required]
        [Column("parche_id")]
        public Guid ParcheId { get; set; }

        [Required]
        [StringLength(1)]
        [Column("dia")]
        public string Dia { get; set; } = null!;

        [Required]
        [StringLength(5)]
        [Column("hora_inicio")]
        public string HoraInicio { get; set; } = null!;

        [Required]
        [StringLength(5)]
        [Column("hora_fin")]
        public string HoraFin { get; set; } = null!;

        [ForeignKey("ParcheId")]
        public virtual Parche Parche { get; set; } = null!;
    }
} 