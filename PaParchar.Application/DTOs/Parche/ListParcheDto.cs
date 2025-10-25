namespace PaParchar.Application.DTOs.Parche
{
    public class ListParcheDto
    {
        public Guid ParcheId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? PortadaUrl { get; set; }
        public decimal? Precio { get; set; }
        public DateTime FechaParche { get; set; }
        public string? Ubicacion { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
    }
}
