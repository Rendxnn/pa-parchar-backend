namespace PaParchar.Application.DTOs.Usuario
{
    public class ListUsuarioDto
    {
        public Guid UserId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
