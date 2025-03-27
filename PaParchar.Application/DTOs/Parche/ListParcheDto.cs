namespace PaParchar.Application.DTOs.Parche
{
    public class ListParcheDto
    {
        public Guid ParcheId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? PortadaUrl { get; set; }
        public DateTime FechaParche { get; set; }
    }
}
