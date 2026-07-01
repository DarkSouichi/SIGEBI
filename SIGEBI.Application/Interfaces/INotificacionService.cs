using SIGEBI.Application.Base;
using SIGEBI.Application.Dtos.Notifications;

namespace SIGEBI.Application.Interfaces
{
    public interface INotificacionService : IBaseService<SaveNotificacionDto, UpdateNotificacionDto, RemoveNotificacionDto>
    {
    }
}