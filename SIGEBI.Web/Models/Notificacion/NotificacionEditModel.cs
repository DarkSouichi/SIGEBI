namespace SIGEBI.Web.Models.Notificacion
{
    public class NotificacionEditModel
    {
        public int id { get; set; }
        public int usuarioId { get; set; }
        public string tipo { get; set; } = string.Empty;
        public string mensaje { get; set; } = string.Empty;
        public string canal { get; set; } = string.Empty;
        public DateTime enviadoEn { get; set; }
    }
}