using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Web.Models.Auth
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El email es requerido.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponseViewModel
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public string token { get; set; } = string.Empty;
        public string nombreCompleto { get; set; } = string.Empty;
        public string rol { get; set; } = string.Empty;
        public int usuarioId { get; set; }
    }
}