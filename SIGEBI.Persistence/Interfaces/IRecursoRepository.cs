using SIGEBI.Domain.Entities.Catalog;
using SIGEBI.Domain.Repository;
using SIGEBI.Domain.Base;

namespace SIGEBI.Persistence.Interfaces
{
    public interface IRecursoRepository : IBaseRepository<Recurso>
    {
        Task<OperationResult> GetEjemplaresByRecursoId(int recursoId);
    }
}