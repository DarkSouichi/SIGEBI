using SIGEBI.Domain.Entities.Users;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;

namespace SIGEBI.Persistence.Repositories.Users
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(LibraryContext context) : base(context)
        {
        }
    }
}