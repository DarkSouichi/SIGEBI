using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Penalties;
using SIGEBI.Domain.Repository;

namespace SIGEBI.Persistence.Interfaces
{
    public interface IPenalizacionRepository : IBaseRepository<Penalizacion>
    {
        Task<OperationResult> GetPenalizacionActivaByUsuarioId(int usuarioId);
        Task<OperationResult> GetPenalizacionesByUsuarioId(int usuarioId);
    }
}