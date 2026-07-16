using SIGEBI.Application.Base;
using SIGEBI.Application.Dtos.Users;
using SIGEBI.Domain.Base;

namespace SIGEBI.Application.Interfaces
{
    public interface IUsuarioService : IBaseService<SaveUsuarioDto, UpdateUsuarioDto, RemoveUsuarioDto>
    {
        Task<OperationResult> GetUsuarioByEmail(string email);
        Task<OperationResult> GetUsuariosActivos();
        Task<OperationResult> VerificarHabilitacion(int usuarioId);
    }
}