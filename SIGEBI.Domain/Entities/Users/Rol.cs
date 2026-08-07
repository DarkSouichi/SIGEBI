using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities.Users
{
    public class Rol : AuditEntity
    {
        public string Nombre { get; set; } = string.Empty;
        public string Permisos { get; set; } = string.Empty;
        public int LimitePrestamos { get; set; } = 3;

        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}