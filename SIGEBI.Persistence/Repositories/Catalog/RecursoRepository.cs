using SIGEBI.Domain.Entities.Catalog;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;

namespace SIGEBI.Persistence.Repositories.Catalog
{
    public class RecursoRepository : BaseRepository<Recurso>
    {
        public RecursoRepository(LibraryContext context) : base(context)
        {
        }
    }
}
