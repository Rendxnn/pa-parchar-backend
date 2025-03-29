using System.ComponentModel.DataAnnotations;

namespace PaParchar.Application.DTOs.Parche
{
    public class ParcheHorarioDto
    {
        [Required]
        [StringLength(1)]
        public string Dia { get; set; } = null!;

        [Required]
        [StringLength(5)]
        public string HoraInicio { get; set; } = null!;

        [Required]
        [StringLength(5)]
        public string HoraFin { get; set; } = null!;
    }
} 