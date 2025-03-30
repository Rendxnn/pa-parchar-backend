using System.ComponentModel.DataAnnotations;

namespace PaParchar.Application.DTOs.Usuario
{
    public class CreateUsuarioDto
    {
        [Required]
        public string Nombre { get; set; } = null!;

        [Required]
        public string Apellido { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Telefono { get; set; } = null!;

        [Required]
        public DateTime FechaNacimiento { get; set; }
    }
}
