using SIGEBI.Web.Models.Notificacion;

namespace SIGEBI.Web.Services
{
    public interface INotificacionApiService
    {
        Task<GetAllNotificacionesResponse> GetAll();
        Task<GetNotificacionResponse> GetById(int id);
        Task<ApiResponse> Create(NotificacionCreateModel model);
    }
}