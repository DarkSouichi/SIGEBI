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
        public int usuarioId { get; set; }
        public int prestamoId { get; set; }
        public decimal monto { get; set; }
        public string estado { get; set; } = string.Empty;
        public DateTime changeDate { get; set; }
        public int changeUser { get; set; }
    }

    public class PenalizacionEditModel
    {
        public int penalizacionId { get; set; }
        public int usuarioId { get; set; }
        public int prestamoId { get; set; }
        public decimal monto { get; set; }
        public string estado { get; set; } = string.Empty;
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