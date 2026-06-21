using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Loans;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;

namespace SIGEBI.Persistence.Repositories.Loans
{
    public class PrestamoRepository : BaseRepository<Prestamo>, IPrestamoRepository
    {
        private readonly LibraryContext _context;

        public PrestamoRepository(LibraryContext context) : base(context)
        {
            _context = context;
        }

        public async Task<OperationResult> GetPrestamosByUsuarioId(int usuarioId)
        {
            OperationResult result = new OperationResult();
            try
            {
                var datos = await _context.Prestamos
                    .Where(p => p.UsuarioId == usuarioId)
                    .ToListAsync();
                result.Data = datos;
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "Ocurrio un error obteniendo los prestamos.";
            }
            return result;
        }
    }
}