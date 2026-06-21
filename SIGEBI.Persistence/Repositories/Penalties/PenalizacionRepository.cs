using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Penalties;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;

namespace SIGEBI.Persistence.Repositories.Penalties
{
    public class PenalizacionRepository : BaseRepository<Penalizacion>, IPenalizacionRepository
    {
        private readonly LibraryContext _context;

        public PenalizacionRepository(LibraryContext context) : base(context)
        {
            _context = context;
        }

        public async Task<OperationResult> GetPenalizacionActivaByUsuarioId(int usuarioId)
        {
            OperationResult result = new OperationResult();
            try
            {
                var dato = await _context.Penalizaciones
                    .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId && p.Estado == "Activa");
                result.Data = dato;
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "Ocurrio un error obteniendo la penalizacion.";
            }
            return result;
        }
    }
}