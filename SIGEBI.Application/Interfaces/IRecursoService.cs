using SIGEBI.Application.Base;
using SIGEBI.Application.Dtos.Catalog;
using SIGEBI.Domain.Base;

namespace SIGEBI.Application.Interfaces
{
    public interface IRecursoService : IBaseService<SaveRecursoDto, UpdateRecursoDto, RemoveRecursoDto>
    {
        Task<OperationResult> GetEjemplaresByRecursoId(int recursoId);
        Task<OperationResult> GetRecursosByCategoria(string categoria);
        Task<OperationResult> GetRecursosDisponibles();
    }
}