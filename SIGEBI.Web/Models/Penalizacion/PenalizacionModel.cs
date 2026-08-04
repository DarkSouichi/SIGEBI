using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SIGEBI.Web.Models.Penalizacion
{
    public class PenalizacionModel
    {
        public int penalizacionId { get; set; }
        public int usuarioId { get; set; }
        public int prestamoId { get; set; }
        public decimal monto { get; set; }
        public string estado { get; set; } = string.Empty;
        public DateTime fechaEmision { get; set; }
    }

    public class GetAllPenalizacionesResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public List<PenalizacionModel> data { get; set; }
    }

    public class GetPenalizacionResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public PenalizacionModel data { get; set; }
    }

    public class PenalizacionCreateModel
    {
        [JsonPropertyName("UsuarioId")]
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un usuario válido.")]
        public int usuarioId { get; set; }

        [JsonPropertyName("PrestamoId")]
        [Required(ErrorMessage = "El préstamo es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un préstamo válido.")]
        public int prestamoId { get; set; }

        [JsonPropertyName("Monto")]
        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0.")]
        public decimal monto { get; set; }

        [JsonPropertyName("Estado")]
        [Required(ErrorMessage = "El estado es obligatorio.")]
        public string estado { get; set; } = string.Empty;

        [JsonPropertyName("FechaEmision")]
        [Required(ErrorMessage = "La fecha de emisión es obligatoria.")]
        public DateTime fechaEmision { get; set; }

        [JsonPropertyName("ChangeDate")]
        public DateTime changeDate { get; set; }

        [JsonPropertyName("ChangeUser")]
        public int changeUser { get; set; }

        public List<SelectListItem> UsuariosList { get; set; } = new();
        public List<SelectListItem> PrestamosList { get; set; } = new();
    }

    public class PenalizacionEditModel
    {
        [JsonPropertyName("Id")]
        [Required(ErrorMessage = "El ID es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID inválido.")]
        public int id { get; set; }

        [JsonPropertyName("UsuarioId")]
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un usuario válido.")]
        public int usuarioId { get; set; }

        [JsonPropertyName("PrestamoId")]
        [Required(ErrorMessage = "El préstamo es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un préstamo válido.")]
        public int prestamoId { get; set; }

        [JsonPropertyName("Monto")]
        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0.")]
        public decimal monto { get; set; }

        [JsonPropertyName("Estado")]
        [Required(ErrorMessage = "El estado es obligatorio.")]
        public string estado { get; set; } = string.Empty;

        [JsonPropertyName("FechaEmision")]
        [Required(ErrorMessage = "La fecha de emisión es obligatoria.")]
        public DateTime fechaEmision { get; set; }

        [JsonPropertyName("ChangeDate")]
        public DateTime changeDate { get; set; }

        [JsonPropertyName("ChangeUser")]
        public int changeUser { get; set; }

        public List<SelectListItem> UsuariosList { get; set; } = new();
        public List<SelectListItem> PrestamosList { get; set; } = new();
    }

    /*public class ApiResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public object? data { get; set; }
    }*/
}