using SIGEBI.Domain.Entities.Users;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;

namespace SIGEBI.Persistence.Repositories.Users
{
    public class UsuarioRepository : BaseRepository<Usuario>
    {
        public UsuarioRepository(LibraryContext context) : base(context)
        {
        }
    }
}