using SIGEBI.Application.Base;
using SIGEBI.Application.Dtos.Notifications;
using SIGEBI.Domain.Base;

namespace SIGEBI.Application.Interfaces
{
    public interface INotificacionService : IBaseService<SaveNotificacionDto, UpdateNotificacionDto, RemoveNotificacionDto>
    {
        Task<OperationResult> GetNotificacionesByUsuarioId(int usuarioId);
    }
}