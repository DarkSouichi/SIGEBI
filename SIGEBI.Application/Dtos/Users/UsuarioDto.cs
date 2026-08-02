using System.ComponentModel.DataAnnotations;
using SIGEBI.Application.Dtos;

namespace SIGEBI.Application.Dtos.Users
{
    public class UsuarioDto : DtoBase
    {
        public int UsuarioId { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EstaActivo { get; set; }
        public int RolId { get; set; }
    }

    public class SaveUsuarioDto : UsuarioDto
    {
        [Required(ErrorMessage = "La contraseña es requerida.")]
        public string Password { get; set; } = string.Empty;
    }

    public class UpdateUsuarioDto
    {
        [Required(ErrorMessage = "El Id es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id debe ser mayor a cero.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre completo es requerido.")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es requerido.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; } = string.Empty;

        public bool EstaActivo { get; set; }

        [Required(ErrorMessage = "El RolId es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "RolId inválido.")]
        public int RolId { get; set; }

        public DateTime ChangeDate { get; set; }
        public int ChangeUser { get; set; }
    }

}