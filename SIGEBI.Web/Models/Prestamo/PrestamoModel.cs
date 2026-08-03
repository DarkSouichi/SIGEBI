using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace SIGEBI.Web.Models.Prestamo
{
    public class PrestamoModel
    {
        public int prestamoId { get; set; }
        public int usuarioId { get; set; }
        public int ejemplarId { get; set; }
        public DateTime fechaPrestamo { get; set; }
        public DateTime fechaDevolucionEsperada { get; set; }
        public DateTime? fechaDevolucionReal { get; set; }
        public string estado { get; set; } = string.Empty;
    }

    public class GetAllPrestamosResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public List<PrestamoModel> data { get; set; }
    }

    public class GetPrestamoResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public PrestamoModel data { get; set; }
    }

    public class PrestamoEditModel
    {
        [JsonPropertyName("Id")]
        public int id { get; set; }

        [JsonPropertyName("UsuarioId")]
        public int usuarioId { get; set; }

        [JsonPropertyName("EjemplarId")]
        public int ejemplarId { get; set; }

        [JsonPropertyName("FechaPrestamo")]
        public DateTime fechaPrestamo { get; set; }

        [JsonPropertyName("FechaDevolucionEsperada")]
        public DateTime fechaDevolucionEsperada { get; set; }

        [JsonPropertyName("FechaDevolucionReal")]
        public DateTime? fechaDevolucionReal { get; set; }

        [JsonPropertyName("Estado")]
        public string estado { get; set; } = string.Empty;

        [JsonPropertyName("ChangeDate")]
        public DateTime changeDate { get; set; }

        [JsonPropertyName("ChangeUser")]
        public int changeUser { get; set; }

        public List<SelectListItem> UsuariosList { get; set; } = new();
        public List<SelectListItem> EjemplaresList { get; set; } = new();
    }

    public class PrestamoCreateModel
    {
        [JsonPropertyName("UsuarioId")]
        public int usuarioId { get; set; }

        [JsonPropertyName("EjemplarId")]
        public int ejemplarId { get; set; }

        [JsonPropertyName("FechaPrestamo")]
        public DateTime fechaPrestamo { get; set; }

        [JsonPropertyName("FechaDevolucionEsperada")]
        public DateTime fechaDevolucionEsperada { get; set; }

        [JsonPropertyName("FechaDevolucionReal")]
        public DateTime? fechaDevolucionReal { get; set; }

        [JsonPropertyName("Estado")]
        public string estado { get; set; } = string.Empty;

        [JsonPropertyName("ChangeDate")]
        public DateTime changeDate { get; set; }

        [JsonPropertyName("ChangeUser")]
        public int changeUser { get; set; }

        public List<SelectListItem> UsuariosList { get; set; } = new();
        public List<SelectListItem> EjemplaresList { get; set; } = new();
    }

    public class ApiResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public object? data { get; set; }
    }
}