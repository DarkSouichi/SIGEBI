using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities.Notifications;

public class Notificacion : AuditEntity
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public DateTime EnviadoEn { get; set; }
    public string Canal { get; set; } = "Email";
}