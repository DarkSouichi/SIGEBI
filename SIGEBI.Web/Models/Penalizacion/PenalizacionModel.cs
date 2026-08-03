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
        public int usuarioId { get; set; }

        [JsonPropertyName("PrestamoId")]
        public int prestamoId { get; set; }

        [JsonPropertyName("Monto")]
        public decimal monto { get; set; }

        [JsonPropertyName("Estado")]
        public string estado { get; set; } = string.Empty;

        [JsonPropertyName("FechaEmision")]
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
        public int id { get; set; }

        [JsonPropertyName("UsuarioId")]
        public int usuarioId { get; set; }

        [JsonPropertyName("PrestamoId")]
        public int prestamoId { get; set; }

        [JsonPropertyName("Monto")]
        public decimal monto { get; set; }

        [JsonPropertyName("Estado")]
        public string estado { get; set; } = string.Empty;

        [JsonPropertyName("FechaEmision")]
        public DateTime fechaEmision { get; set; }

        [JsonPropertyName("ChangeDate")]
        public DateTime changeDate { get; set; }

        [JsonPropertyName("ChangeUser")]
        public int changeUser { get; set; }

        public List<SelectListItem> UsuariosList { get; set; } = new();
        public List<SelectListItem> PrestamosList { get; set; } = new();
    }

    public class ApiResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public object? data { get; set; }
    }
}