using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SIGEBI.Web.Models.Ejemplar
{
    public class EjemplarModel
    {
        [JsonPropertyName("id")]
        public int ejemplarId { get; set; }

        public string codigoBarras { get; set; } = string.Empty;

        public int recursoId { get; set; }
        public string tituloRecurso { get; set; } = string.Empty;

        [JsonPropertyName("estado")] 
        public int estado { get; set; } 

        public string EstadoTexto
        {
            get
            {
                return estado switch
                {
                    0 => "Disponible",
                    1 => "Prestado",
                    2 => "Reservado",
                    3 => "No Disponible",
                    _ => "Desconocido"
                };
            }
        }
    }

    public class GetAllEjemplaresResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public List<EjemplarModel> data { get; set; }
    }

    public class GetEjemplarResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public EjemplarModel data { get; set; }
    }

    public class EjemplarCreateModel
    {
        [JsonPropertyName("CodigoBarras")]
        [Required(ErrorMessage = "El código de barras es obligatorio.")]
        [StringLength(20, ErrorMessage = "El código no puede exceder 20 caracteres.")]
        public string codigoBarras { get; set; } = string.Empty;

        [JsonPropertyName("Estado")]
        [Required(ErrorMessage = "El estado es obligatorio.")]
        [Range(0, 3, ErrorMessage = "Estado inválido (0=Disponible, 1=Prestado, 2=Reservado, 3=No Disponible).")]
        public int estado { get; set; }

        [JsonPropertyName("RecursoId")]
        [Required(ErrorMessage = "El recurso es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un recurso válido.")]
        public int recursoId { get; set; }

        [JsonPropertyName("ChangeDate")]
        public DateTime changeDate { get; set; }

        [JsonPropertyName("ChangeUser")]
        public int changeUser { get; set; }

        public List<SelectListItem> RecursosList { get; set; } = new();
    }

    public class EjemplarEditModel
    {
        [JsonPropertyName("Id")]
        [Required(ErrorMessage = "El ID es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID inválido.")]
        public int id { get; set; }

        [JsonPropertyName("CodigoBarras")]
        [Required(ErrorMessage = "El código de barras es obligatorio.")]
        [StringLength(20, ErrorMessage = "El código no puede exceder 20 caracteres.")]
        public string codigoBarras { get; set; } = string.Empty;

        [JsonPropertyName("Estado")]
        [Required(ErrorMessage = "El estado es obligatorio.")]
        [Range(0, 3, ErrorMessage = "Estado inválido (0=Disponible, 1=Prestado, 2=Reservado, 3=No Disponible).")]
        public int estado { get; set; }

        [JsonPropertyName("RecursoId")]
        [Required(ErrorMessage = "El recurso es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un recurso válido.")]
        public int recursoId { get; set; }

        [JsonPropertyName("ChangeDate")]
        public DateTime changeDate { get; set; }

        [JsonPropertyName("ChangeUser")]
        public int changeUser { get; set; }

        public List<SelectListItem> RecursosList { get; set; } = new();
    }
}