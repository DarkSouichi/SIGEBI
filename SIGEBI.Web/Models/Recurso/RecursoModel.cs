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
        public int id { get; set; }

        [JsonPropertyName("Titulo")]
        public string titulo { get; set; } = string.Empty;

        [JsonPropertyName("Autor")]
        public string autor { get; set; } = string.Empty;

        [JsonPropertyName("ISBN")]
        public string isbn { get; set; } = string.Empty;

        [JsonPropertyName("Categoria")]
        public string categoria { get; set; } = string.Empty;

        [JsonPropertyName("ChangeDate")]
        public DateTime changeDate { get; set; }

        [JsonPropertyName("ChangeUser")]
        public int changeUser { get; set; }
    }

    public class RecursoCreateModel
    {
        [JsonPropertyName("Titulo")]
        public string titulo { get; set; } = string.Empty;

        [JsonPropertyName("Autor")]
        public string autor { get; set; } = string.Empty;

        [JsonPropertyName("ISBN")]
        public string isbn { get; set; } = string.Empty;

        [JsonPropertyName("Categoria")]
        public string categoria { get; set; } = string.Empty;

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