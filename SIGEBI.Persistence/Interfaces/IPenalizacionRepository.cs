using SIGEBI.Domain.Entities.Penalties;
using SIGEBI.Domain.Repository;
using SIGEBI.Domain.Base;

namespace SIGEBI.Persistence.Interfaces
{
    public interface IPenalizacionRepository : IBaseRepository<Penalizacion>
    {
        Task<OperationResult> GetPenalizacionActivaByUsuarioId(int usuarioId);
    }
}