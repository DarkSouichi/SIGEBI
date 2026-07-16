using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Notifications;
using SIGEBI.Domain.Repository;

namespace SIGEBI.Persistence.Interfaces
{
    public interface INotificacionRepository : IBaseRepository<Notificacion>
    {
        Task<OperationResult> GetNotificacionesByUsuarioId(int usuarioId);
    }
}