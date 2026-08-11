using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace SIGEBI.Web.Models.Prestamo
{
    public class PrestamoModel
    {
        public int prestamoId { get; set; }
        public int usuarioId { get; set; }
        public string nombreUsuario { get; set; } = string.Empty;  
        public int ejemplarId { get; set; }
        public string codigoEjemplar { get; set; } = string.Empty;
        public string tituloRecurso { get; set; } = string.Empty;
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
        [Required(ErrorMessage = "El ID es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID inválido.")]
        public int id { get; set; }

        [JsonPropertyName("UsuarioId")]
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un usuario válido.")]
        public int usuarioId { get; set; }

        [JsonPropertyName("EjemplarId")]
        [Required(ErrorMessage = "El ejemplar es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un ejemplar válido.")]
        public int ejemplarId { get; set; }

        [JsonPropertyName("FechaPrestamo")]
        [Required(ErrorMessage = "La fecha de préstamo es obligatoria.")]
        public DateTime fechaPrestamo { get; set; }

        [JsonPropertyName("FechaDevolucionEsperada")]
        [Required(ErrorMessage = "La fecha de devolución esperada es obligatoria.")]
        public DateTime fechaDevolucionEsperada { get; set; }

        [JsonPropertyName("FechaDevolucionReal")]
        public DateTime? fechaDevolucionReal { get; set; }

        [JsonPropertyName("Estado")]
        [Required(ErrorMessage = "El estado es obligatorio.")]
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
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un usuario válido.")]
        public int usuarioId { get; set; }

        [JsonPropertyName("EjemplarId")]
        [Required(ErrorMessage = "El ejemplar es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un ejemplar válido.")]
        public int ejemplarId { get; set; }

        [JsonPropertyName("FechaPrestamo")]
        [Required(ErrorMessage = "La fecha de préstamo es obligatoria.")]
        public DateTime fechaPrestamo { get; set; }

        [JsonPropertyName("FechaDevolucionEsperada")]
        [Required(ErrorMessage = "La fecha de devolución esperada es obligatoria.")]
        public DateTime fechaDevolucionEsperada { get; set; }

        [JsonPropertyName("Estado")]
        [Required(ErrorMessage = "El estado es obligatorio.")]
        public string estado { get; set; } = string.Empty;

        [JsonPropertyName("ChangeDate")]
        public DateTime changeDate { get; set; }

        [JsonPropertyName("ChangeUser")]
        public int changeUser { get; set; }

        public List<SelectListItem> UsuariosList { get; set; } = new();
        public List<SelectListItem> EjemplaresList { get; set; } = new();
    }

    /*public class ApiResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public object? data { get; set; }
    }*/
}