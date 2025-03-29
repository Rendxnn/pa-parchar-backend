using System.ComponentModel.DataAnnotations;

namespace PaParchar.Application.DTOs.Parche
{
    public class CreateParcheDto
    {
        [Required]
        public string Nombre { get; set; } = null!;

        public string? Descripcion { get; set; }

        public string? Ubicacion { get; set; }

        public int? Capacidad { get; set; }

        public bool EsPrivado { get; set; } = false;

        public decimal? Precio { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        public ICollection<ParcheHorarioDto>? Horarios { get; set; }
    }
}
