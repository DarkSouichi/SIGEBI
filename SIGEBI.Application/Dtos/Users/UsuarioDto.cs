using SIGEBI.Application.Dtos;

namespace SIGEBI.Application.Dtos.Users
{
    public class UsuarioDto : DtoBase
    {
        public int UsuarioId { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EstaActivo { get; set; }
        public int RolId { get; set; }
    }

    public class SaveUsuarioDto : UsuarioDto
    {
        public string Password { get; set; } = string.Empty;
    }

    public class UpdateUsuarioDto : UsuarioDto
    {
        public int Id { get; set; }
    }

    public class RemoveUsuarioDto : DtoBase
    {
        public int Id { get; set; }
        public bool Removed { get; set; }
    }
}