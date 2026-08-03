using SIGEBI.Domain.Entities.Catalog;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;

namespace SIGEBI.Persistence.Repositories.Catalog
{
    public class EjemplarRepository : BaseRepository<Ejemplar>, IEjemplarRepository
    {
        public EjemplarRepository(LibraryContext context) : base(context)
        {
        }
    }
}