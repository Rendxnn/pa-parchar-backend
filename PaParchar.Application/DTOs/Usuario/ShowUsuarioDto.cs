using PaParchar.Domain.Entities;

namespace PaParchar.Application.DTOs.Usuario
{
    public class ShowUsuarioDto
    {
        public Guid UserId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }
    }
}
