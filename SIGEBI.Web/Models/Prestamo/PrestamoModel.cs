namespace SIGEBI.Web.Models.Prestamo
{
    public class PrestamoModel
    {
        public int prestamoId { get; set; }
        public int usuarioId { get; set; }
        public int ejemplarId { get; set; }
        public DateTime fechaPrestamo { get; set; }
        public DateTime fechaDevolucionEsperada { get; set; }
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
        public int prestamoId { get; set; }
        public int usuarioId { get; set; }
        public int ejemplarId { get; set; }
        public DateTime fechaDevolucionEsperada { get; set; }
        public string estado { get; set; } = string.Empty;
        public DateTime changeDate { get; set; }
        public int changeUser { get; set; }
    }

    public class PrestamoCreateModel
    {
        public int usuarioId { get; set; }
        public int ejemplarId { get; set; }
        public DateTime fechaPrestamo { get; set; }
        public DateTime fechaDevolucionEsperada { get; set; }
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