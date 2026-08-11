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

        public async Task<List<Recurso>> GetAllWithEjemplaresAsync()
        {
            return await _context.Recursos
                .Include(r => r.Ejemplares)
                .ToListAsync();
        }

        public async Task<Recurso?> GetByIdWithEjemplaresAsync(int id)
        {
            return await _context.Recursos
                .Include(r => r.Ejemplares)
                .FirstOrDefaultAsync(r => r.Id == id);
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
                result.Message = "Ocurrió un error obteniendo los ejemplares del recurso.";
            }
            return result;
        }

        public async Task<OperationResult> GetRecursosByCategoria(string categoria)
        {
            OperationResult result = new OperationResult();
            try
            {
                var datos = await _context.Recursos
                    .Where(r => r.Categoria == categoria)
                    .ToListAsync();
                result.Data = datos;
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "Ocurrió un error obteniendo los recursos por categoría.";
            }
            return result;
        }

        public async Task<OperationResult> GetRecursosDisponibles()
        {
            OperationResult result = new OperationResult();
            try
            {
                var datos = await _context.Recursos
                    .Where(r => r.Ejemplares.Any(e => e.Estado == EstadoEjemplar.Disponible))
                    .ToListAsync();
                result.Data = datos;
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "Ocurrió un error obteniendo los recursos disponibles.";
            }
            return result;
        }
    }
}