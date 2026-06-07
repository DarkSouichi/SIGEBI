using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities.Users;

public class Usuario : AuditEntity
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool EstaActivo { get; set; } = true;
    public int RolId { get; set; }
    public int IntentosFallidos { get; set; } = 0;
}

public class Rol
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Permisos { get; set; } = string.Empty;
}