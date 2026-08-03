using SIGEBI.Domain.Entities.Users;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;

namespace SIGEBI.Persistence.Repositories.Users
{
    public class RolRepository : BaseRepository<Rol>, IRolRepository
    {
        public RolRepository(LibraryContext context) : base(context)
        {
        }
    }
}