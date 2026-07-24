namespace SIGEBI.Web.Models.Recurso
{
    public class RecursoModel
    {
        public int recursoId { get; set; }
        public string titulo { get; set; } = string.Empty;
        public string autor { get; set; } = string.Empty;
        public string isbn { get; set; } = string.Empty;
        public string categoria { get; set; } = string.Empty;
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
        public int recursoId { get; set; }
        public string titulo { get; set; } = string.Empty;
        public string autor { get; set; } = string.Empty;
        public string isbn { get; set; } = string.Empty;
        public string categoria { get; set; } = string.Empty;
        public DateTime changeDate { get; set; }
        public int changeUser { get; set; }
    }

    public class RecursoCreateModel
    {
        public string titulo { get; set; } = string.Empty;
        public string autor { get; set; } = string.Empty;
        public string isbn { get; set; } = string.Empty;
        public string categoria { get; set; } = string.Empty;
        public DateTime changeDate { get; set; }
        public int changeUser { get; set; }
    }

    public class ApiResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public object? data { get; set; }
    }
}