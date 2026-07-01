using SIGEBI.Application.Dtos;

namespace SIGEBI.Application.Dtos.Notifications
{
    public class NotificacionDto : DtoBase
    {
        public int NotificacionId { get; set; }
        public int UsuarioId { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public DateTime EnviadoEn { get; set; }
        public string Canal { get; set; } = string.Empty;
    }

    public class SaveNotificacionDto : NotificacionDto
    {
    }

    public class UpdateNotificacionDto : NotificacionDto
    {
        public int Id { get; set; }
    }

    public class RemoveNotificacionDto : DtoBase
    {
        public int Id { get; set; }
        public bool Removed { get; set; }
    }
}