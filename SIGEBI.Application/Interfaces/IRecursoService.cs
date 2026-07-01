using SIGEBI.Application.Base;
using SIGEBI.Application.Dtos.Catalog;

namespace SIGEBI.Application.Interfaces
{
    public interface IRecursoService : IBaseService<SaveRecursoDto, UpdateRecursoDto, RemoveRecursoDto>
    {
    }
}