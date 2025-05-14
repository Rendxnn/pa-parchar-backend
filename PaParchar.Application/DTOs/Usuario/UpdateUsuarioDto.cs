namespace PaParchar.Application.DTOs.Usuario
{
    public class UpdateUsuarioDto
    {
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Telefono { get; set; } = null!;
    }
}
