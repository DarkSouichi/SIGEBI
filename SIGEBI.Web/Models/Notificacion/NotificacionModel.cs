using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SIGEBI.Web.Models.Notificacion
{
    public class NotificacionModel
    {
        public int notificacionId { get; set; }
        public int usuarioId { get; set; }
        public string tipo { get; set; } = string.Empty;
        public string mensaje { get; set; } = string.Empty;
        public DateTime enviadoEn { get; set; }
        public string canal { get; set; } = string.Empty;
    }

    public class GetAllNotificacionesResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public List<NotificacionModel> data { get; set; }
    }

    public class GetNotificacionResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public NotificacionModel data { get; set; }
    }

    public class NotificacionCreateModel
    {
        [JsonPropertyName("UsuarioId")]
        public int usuarioId { get; set; }

        [JsonPropertyName("Tipo")]
        public string tipo { get; set; } = string.Empty;

        [JsonPropertyName("Mensaje")]
        public string mensaje { get; set; } = string.Empty;

        [JsonPropertyName("Canal")]
        public string canal { get; set; } = string.Empty;

        [JsonPropertyName("EnviadoEn")]
        public DateTime enviadoEn { get; set; }

        [JsonPropertyName("ChangeDate")]
        public DateTime changeDate { get; set; }

        [JsonPropertyName("ChangeUser")]
        public int changeUser { get; set; }

        public List<SelectListItem> UsuariosList { get; set; } = new();
    }

    public class NotificacionEditModel
    {
        [JsonPropertyName("Id")]
        public int id { get; set; }

        [JsonPropertyName("UsuarioId")]
        public int usuarioId { get; set; }

        [JsonPropertyName("Tipo")]
        public string tipo { get; set; } = string.Empty;

        [JsonPropertyName("Mensaje")]
        public string mensaje { get; set; } = string.Empty;

        [JsonPropertyName("Canal")]
        public string canal { get; set; } = string.Empty;

        [JsonPropertyName("EnviadoEn")]
        public DateTime enviadoEn { get; set; }

        [JsonPropertyName("ChangeDate")]
        public DateTime changeDate { get; set; }

        [JsonPropertyName("ChangeUser")]
        public int changeUser { get; set; }

        public List<SelectListItem> UsuariosList { get; set; } = new();
    }

    public class ApiResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public object? data { get; set; }
    }
}