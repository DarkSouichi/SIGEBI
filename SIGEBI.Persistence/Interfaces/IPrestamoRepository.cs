using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Loans;
using SIGEBI.Domain.Repository;

namespace SIGEBI.Persistence.Interfaces
{
    public interface IPrestamoRepository : IBaseRepository<Prestamo>
    {
        Task<OperationResult> GetPrestamosByUsuarioId(int usuarioId);
        Task<OperationResult> GetPrestamosActivos();
        Task<OperationResult> GetPrestamosByEjemplarId(int ejemplarId);
    }
}