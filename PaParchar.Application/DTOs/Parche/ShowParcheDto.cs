using PaParchar.Domain.Entities;

namespace PaParchar.Application.DTOs.Parche
{
    public class ShowParcheDto
    {
        public Guid ParcheId { get; set; }
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
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? UltimaActualizacion { get; set; }
        public ICollection<CreateParcheHorarioDto>? Horarios { get; set; }
        public ICollection<ParcheImagenDto>? Imagenes { get; set; }
    }
}
