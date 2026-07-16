using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Notifications;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;

namespace SIGEBI.Persistence.Repositories.Notifications
{
    public class NotificacionRepository : BaseRepository<Notificacion>, INotificacionRepository
    {
        private readonly LibraryContext _context;

        public NotificacionRepository(LibraryContext context) : base(context)
        {
            _context = context;
        }

        public async Task<OperationResult> GetNotificacionesByUsuarioId(int usuarioId)
        {
            OperationResult result = new OperationResult();
            try
            {
                var datos = await _context.Notificaciones
                    .Where(n => n.UsuarioId == usuarioId)
                    .OrderByDescending(n => n.EnviadoEn)
                    .ToListAsync();
                result.Data = datos;
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "Ocurrio un error obteniendo las notificaciones del usuario.";
            }
            return result;
        }
    }
}