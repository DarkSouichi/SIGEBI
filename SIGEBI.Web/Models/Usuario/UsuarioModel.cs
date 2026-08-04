using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SIGEBI.Web.Models.Usuario

{
    public class UsuarioModel
    {
        public int usuarioId { get; set; }
        public string nombreCompleto { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public bool estaActivo { get; set; }
        public int rolId { get; set; }
    }

    public class GetAllUsuariosResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public List<UsuarioModel> data { get; set; }
    }

    public class GetUsuarioResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public UsuarioModel data { get; set; }
    }

    public class UsuarioEditModel
    {
        [JsonPropertyName("Id")]
        [Required(ErrorMessage = "El ID es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID inválido.")]
        public int usuarioId { get; set; }

        [JsonPropertyName("NombreCompleto")]
        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres.")]
        public string nombreCompleto { get; set; } = string.Empty;

        [JsonPropertyName("Email")]
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ser un email válido.")]
        public string email { get; set; } = string.Empty;

        [JsonPropertyName("EstaActivo")]
        public bool estaActivo { get; set; }

        [JsonPropertyName("RolId")]
        [Required(ErrorMessage = "El rol es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un rol válido.")]
        public int rolId { get; set; }

        [JsonPropertyName("ChangeDate")]
        public DateTime changeDate { get; set; }

        [JsonPropertyName("ChangeUser")]
        public int changeUser { get; set; }
    }

    public class UsuarioCreateModel
    {
        [JsonPropertyName("NombreCompleto")]
        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres.")]
        public string nombreCompleto { get; set; } = string.Empty;

        [JsonPropertyName("Email")]
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ser un email válido.")]
        public string email { get; set; } = string.Empty;

        [JsonPropertyName("Password")]
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string password { get; set; } = string.Empty;

        [JsonPropertyName("EstaActivo")]
        public bool estaActivo { get; set; } = true;

        [JsonPropertyName("RolId")]
        [Required(ErrorMessage = "El rol es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un rol válido.")]
        public int rolId { get; set; }

        [JsonPropertyName("ChangeDate")]
        public DateTime changeDate { get; set; }

        [JsonPropertyName("ChangeUser")]
        public int changeUser { get; set; }
    }

   /*public class ApiResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public object? data { get; set; }
    }*/
}