using SIGEBI.Application.Base;
using SIGEBI.Application.Dtos.Penalties;

namespace SIGEBI.Application.Interfaces
{
    public interface IPenalizacionService : IBaseService<SavePenalizacionDto, UpdatePenalizacionDto, RemovePenalizacionDto>
    {
    }
}