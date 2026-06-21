using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Catalog;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;

namespace SIGEBI.Persistence.Repositories.Catalog
{
    public class RecursoRepository : BaseRepository<Recurso>, IRecursoRepository
    {
        private readonly LibraryContext _context;

        public RecursoRepository(LibraryContext context) : base(context)
        {
            _context = context;
        }

        public async Task<OperationResult> GetEjemplaresByRecursoId(int recursoId)
        {
            OperationResult result = new OperationResult();
            try
            {
                var datos = await _context.Ejemplares
                    .Where(e => e.RecursoId == recursoId)
                    .ToListAsync();
                result.Data = datos;
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "Ocurrio un error obteniendo los ejemplares.";
            }
            return result;
        }
    }
}