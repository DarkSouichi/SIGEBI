using SIGEBI.Domain.Entities.Loans;
using SIGEBI.Domain.Repository;
using SIGEBI.Domain.Base;

namespace SIGEBI.Persistence.Interfaces
{
    public interface IPrestamoRepository : IBaseRepository<Prestamo>
    {
        Task<OperationResult> GetPrestamosByUsuarioId(int usuarioId);
    }
}