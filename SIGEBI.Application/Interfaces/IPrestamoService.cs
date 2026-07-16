using SIGEBI.Application.Base;
using SIGEBI.Application.Dtos.Loans;
using SIGEBI.Domain.Base;

namespace SIGEBI.Application.Interfaces
{
    public interface IPrestamoService : IBaseService<SavePrestamoDto, UpdatePrestamoDto, RemovePrestamoDto>
    {
        Task<OperationResult> GetPrestamosByUsuarioId(int usuarioId);
        Task<OperationResult> GetPrestamosActivos();
        Task<OperationResult> GetPrestamosByEjemplarId(int ejemplarId);
    }
}