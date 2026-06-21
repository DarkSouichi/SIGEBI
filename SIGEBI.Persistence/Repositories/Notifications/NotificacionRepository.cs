using SIGEBI.Domain.Entities.Notifications;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;

namespace SIGEBI.Persistence.Repositories.Notifications
{
    public class NotificacionRepository : BaseRepository<Notificacion>
    {
        public NotificacionRepository(LibraryContext context) : base(context)
        {
        }
    }
}
