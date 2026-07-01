using SIGEBI.Application.Base;
using SIGEBI.Application.Dtos.Loans;

namespace SIGEBI.Application.Interfaces
{
    public interface IPrestamoService : IBaseService<SavePrestamoDto, UpdatePrestamoDto, RemovePrestamoDto>
    {
    }
}