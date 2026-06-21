using SIGEBI.Domain.Entities.Loans;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;

namespace SIGEBI.Persistence.Repositories.Loans
{
    public class PrestamoRepository : BaseRepository<Prestamo>
    {
        public PrestamoRepository(LibraryContext context) : base(context)
        {
        }
    }
}