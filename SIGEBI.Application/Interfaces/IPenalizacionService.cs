using SIGEBI.Application.Base;
using SIGEBI.Application.Dtos.Penalties;
using SIGEBI.Domain.Base;

namespace SIGEBI.Application.Interfaces
{
    public interface IPenalizacionService : IBaseService<SavePenalizacionDto, UpdatePenalizacionDto>
    {
        Task<OperationResult> GetPenalizacionActivaByUsuarioId(int usuarioId);
        Task<OperationResult> GetPenalizacionesByUsuarioId(int usuarioId);
    }
}