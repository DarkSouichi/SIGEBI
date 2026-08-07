using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SIGEBI.Web.Models.Notificacion
{
    public class NotificacionModel
    {
        public int notificacionId { get; set; }
        public int usuarioId { get; set; }
        public string nombreUsuario { get; set; } = string.Empty;
        public string tipo { get; set; } = string.Empty;
        public string mensaje { get; set; } = string.Empty;
        public DateTime enviadoEn { get; set; }
        public string canal { get; set; } = string.Empty;
        public int? prestamoId { get; set; }
        public int? recursoId { get; set; }
        public bool leida { get; set; }
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
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un usuario válido.")]
        public int usuarioId { get; set; }

        [JsonPropertyName("Tipo")]
        [Required(ErrorMessage = "El tipo es obligatorio.")]
        [StringLength(50, ErrorMessage = "El tipo no puede exceder 50 caracteres.")]
        public string tipo { get; set; } = string.Empty;

        [JsonPropertyName("Mensaje")]
        [Required(ErrorMessage = "El mensaje es obligatorio.")]
        public string mensaje { get; set; } = string.Empty;

        [JsonPropertyName("Canal")]
        [Required(ErrorMessage = "El canal es obligatorio.")]
        [StringLength(50, ErrorMessage = "El canal no puede exceder 50 caracteres.")]
        public string canal { get; set; } = string.Empty;

        [JsonPropertyName("ChangeDate")]
        public DateTime changeDate { get; set; }

        [JsonPropertyName("ChangeUser")]
        public int changeUser { get; set; }

        public List<SelectListItem> UsuariosList { get; set; } = new();
    }

    public class NotificacionEditModel
    {
        [JsonPropertyName("Id")]
        [Required(ErrorMessage = "El ID es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID inválido.")]
        public int id { get; set; }

        [JsonPropertyName("UsuarioId")]
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un usuario válido.")]
        public int usuarioId { get; set; }

        [JsonPropertyName("Tipo")]
        [Required(ErrorMessage = "El tipo es obligatorio.")]
        [StringLength(50, ErrorMessage = "El tipo no puede exceder 50 caracteres.")]
        public string tipo { get; set; } = string.Empty;

        [JsonPropertyName("Mensaje")]
        [Required(ErrorMessage = "El mensaje es obligatorio.")]
        public string mensaje { get; set; } = string.Empty;

        [JsonPropertyName("Canal")]
        [Required(ErrorMessage = "El canal es obligatorio.")]
        [StringLength(50, ErrorMessage = "El canal no puede exceder 50 caracteres.")]
        public string canal { get; set; } = string.Empty;

        [JsonPropertyName("ChangeDate")]
        public DateTime changeDate { get; set; }

        [JsonPropertyName("ChangeUser")]
        public int changeUser { get; set; }

        public List<SelectListItem> UsuariosList { get; set; } = new();
    }

    /*public class ApiResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public object? data { get; set; }
    }*/
}