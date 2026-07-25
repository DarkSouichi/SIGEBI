namespace SIGEBI.Web.Models.Notificacion
{
    public class NotificacionModel
    {
        public int notificacionId { get; set; }
        public int usuarioId { get; set; }
        public string tipo { get; set; } = string.Empty;
        public string mensaje { get; set; } = string.Empty;
        public DateTime enviadoEn { get; set; }
        public string canal { get; set; } = string.Empty;
    }

    public class GetAllNotificacionesResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public List<NotificacionModel> data { get; set; }
    }

    public class GetNotificacionResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        public NotificacionModel data { get; set; }
    }

    public class NotificacionCreateModel
    {
        public int usuarioId { get; set; }
        public string tipo { get; set; } = string.Empty;
        public string mensaje { get; set; } = string.Empty;
        public string canal { get; set; } = string.Empty;
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