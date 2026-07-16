using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Catalog;
using SIGEBI.Domain.Repository;

namespace SIGEBI.Persistence.Interfaces
{
    public interface IRecursoRepository : IBaseRepository<Recurso>
    {
        Task<OperationResult> GetEjemplaresByRecursoId(int recursoId);
        Task<OperationResult> GetRecursosByCategoria(string categoria);
        Task<OperationResult> GetRecursosDisponibles();
    }
}