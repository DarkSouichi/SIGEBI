using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Users;
using SIGEBI.Domain.Repository;

namespace SIGEBI.Persistence.Interfaces
{
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        Task<OperationResult> GetUsuarioByEmail(string email);
        Task<OperationResult> GetUsuariosActivos();
        Task<OperationResult> VerificarHabilitacion(int usuarioId);
    }
}