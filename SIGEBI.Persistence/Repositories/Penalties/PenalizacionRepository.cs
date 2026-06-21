using SIGEBI.Domain.Entities.Penalties;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;

namespace SIGEBI.Persistence.Repositories.Penalties
{
    public class PenalizacionRepository : BaseRepository<Penalizacion>
    {
        public PenalizacionRepository(LibraryContext context) : base(context)
        {
        }
    }
}
