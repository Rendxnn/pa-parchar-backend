using PaParchar.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace PaParchar.Application.DTOs.Parche
{
    public class UpdateParcheDto
    {
        public string Nombre { get; set; } = null!;

        public string? Descripcion { get; set; }

        public string? Ubicacion { get; set; }

        public int? Capacidad { get; set; }

        public bool EsPrivado { get; set; } = false;

        public decimal? Precio { get; set; }

        public string? PortadaUrl { get; set; }
        
        public decimal? Latitud { get; set; }
        
        public decimal? Longitud { get; set; }

        public EstadoParche Estado { get; set; } = EstadoParche.Creado;

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        public ICollection<CreateParcheHorarioDto>? Horarios { get; set; }
    }
} 