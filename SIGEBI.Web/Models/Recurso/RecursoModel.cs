using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SIGEBI.Web.Models.Ejemplar;

namespace SIGEBI.Web.Models.Recurso
{
    public class RecursoModel
    {
        public int recursoId { get; set; }
        public string titulo { get; set; } = string.Empty;
        public string autor { get; set; } = string.Empty;
        public string isbn { get; set; } = string.Empty;
        public string categoria { get; set; } = string.Empty;
        public List<EjemplarModel> Ejemplares { get; set; } = new();
    }

    public class GetAllRecursosResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public List<RecursoModel> data { get; set; }
    }

    public class GetRecursoResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public RecursoModel data { get; set; }
    }

    public class RecursoEditModel
    {
        [JsonPropertyName("Id")]
        [Required(ErrorMessage = "El ID es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID inválido.")]
        public int id { get; set; }

        [JsonPropertyName("Titulo")]
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(200, ErrorMessage = "El título no puede exceder 200 caracteres.")]
        public string titulo { get; set; } = string.Empty;

        [JsonPropertyName("Autor")]
        [Required(ErrorMessage = "El autor es obligatorio.")]
        [StringLength(100, ErrorMessage = "El autor no puede exceder 100 caracteres.")]
        public string autor { get; set; } = string.Empty;

        [JsonPropertyName("ISBN")]
        [Required(ErrorMessage = "El ISBN es obligatorio.")]
        [StringLength(20, ErrorMessage = "El ISBN no puede exceder 20 caracteres.")]
        public string isbn { get; set; } = string.Empty;

        [JsonPropertyName("Categoria")]
        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [StringLength(50, ErrorMessage = "La categoría no puede exceder 50 caracteres.")]
        public string categoria { get; set; } = string.Empty;

        [JsonPropertyName("ChangeDate")]
        public DateTime changeDate { get; set; }

        [JsonPropertyName("ChangeUser")]
        public int changeUser { get; set; }
    }

    public class RecursoCreateModel
    {
        [JsonPropertyName("Titulo")]
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(200, ErrorMessage = "El título no puede exceder 200 caracteres.")]
        public string titulo { get; set; } = string.Empty;

        [JsonPropertyName("Autor")]
        [Required(ErrorMessage = "El autor es obligatorio.")]
        [StringLength(100, ErrorMessage = "El autor no puede exceder 100 caracteres.")]
        public string autor { get; set; } = string.Empty;

        [JsonPropertyName("ISBN")]
        [Required(ErrorMessage = "El ISBN es obligatorio.")]
        [StringLength(20, ErrorMessage = "El ISBN no puede exceder 20 caracteres.")]
        public string isbn { get; set; } = string.Empty;

        [JsonPropertyName("Categoria")]
        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [StringLength(50, ErrorMessage = "La categoría no puede exceder 50 caracteres.")]
        public string categoria { get; set; } = string.Empty;

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