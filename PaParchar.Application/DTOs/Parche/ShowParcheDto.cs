using PaParchar.Domain.Entities;

namespace PaParchar.Application.DTOs.Parche
{
    public class ShowParcheDto
    {
        public Guid ParcheId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? Ubicacion { get; set; }
        public int? Duracion { get; set; }
        public int? Capacidad { get; set; }
        public bool EsPrivado { get; set; } = false;
        public decimal? Precio { get; set; }
        public string? PortadaUrl { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        public EstadoParche Estado { get; set; } = EstadoParche.Creado;
        public DateTime FechaParche { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? UltimaActualizacion { get; set; }
    }
}
