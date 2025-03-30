using System.ComponentModel.DataAnnotations;

namespace PaParchar.Application.DTOs.Parche
{
    public class CreateParcheHorarioDto
    {
        [Required]
        [StringLength(1)]
        public string Dia { get; set; } = null!;

        [Required]
        public TimeOnly HoraInicio { get; set; }

        [Required]
        public TimeOnly HoraFin { get; set; }
    }
} 