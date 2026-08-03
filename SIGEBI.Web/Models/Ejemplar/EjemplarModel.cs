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
        public string codigoBarras { get; set; } = string.Empty;
        public int estado { get; set; } 
        public int recursoId { get; set; }
        public DateTime changeDate { get; set; }
        public int changeUser { get; set; }

        public List<SelectListItem> RecursosList { get; set; } = new();
    }

    public class EjemplarEditModel
    {
        public int id { get; set; }
        public string codigoBarras { get; set; } = string.Empty;
        public int estado { get; set; }
        public int recursoId { get; set; }
        public DateTime changeDate { get; set; }
        public int changeUser { get; set; }

        public List<SelectListItem> RecursosList { get; set; } = new();
    }
}