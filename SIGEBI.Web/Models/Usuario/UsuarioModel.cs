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
        public int usuarioId { get; set; } 

        [JsonPropertyName("NombreCompleto")]
        public string nombreCompleto { get; set; }

        [JsonPropertyName("Email")]
        public string email { get; set; }

        [JsonPropertyName("EstaActivo")]
        public bool estaActivo { get; set; }

        [JsonPropertyName("RolId")]
        public int rolId { get; set; }

        [JsonPropertyName("ChangeDate")]
        public DateTime changeDate { get; set; }

        [JsonPropertyName("ChangeUser")]
        public int changeUser { get; set; }
    }

    public class UsuarioCreateModel
    {
        [JsonPropertyName("NombreCompleto")]
        public string nombreCompleto { get; set; } = string.Empty;

        [JsonPropertyName("Email")]
        public string email { get; set; } = string.Empty;

        [JsonPropertyName("Password")]
        public string password { get; set; } = string.Empty;

        [JsonPropertyName("EstaActivo")]
        public bool estaActivo { get; set; }

        [JsonPropertyName("RolId")]
        public int rolId { get; set; }

        [JsonPropertyName("ChangeDate")]
        public DateTime changeDate { get; set; }

        [JsonPropertyName("ChangeUser")]
        public int changeUser { get; set; }
    }

    public class ApiResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public object? data { get; set; }
    }
}