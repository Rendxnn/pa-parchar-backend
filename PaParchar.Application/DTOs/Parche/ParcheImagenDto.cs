namespace PaParchar.Application.DTOs.Parche
{
    public class ParcheImagenDto
    {
        public Guid Id { get; set; }
        public string ImagenUrl { get; set; } = null!;
        public string? Descripcion { get; set; }
        public int Orden { get; set; }
    }
} 