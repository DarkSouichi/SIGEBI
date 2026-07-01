using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;
using SIGEBI.Persistence.Interfaces;
using SIGEBI.Persistence.Repositories.Notifications;

namespace SIGEBI.IOC.Dependencies
{
    public static class NotificacionDependency
    {
        public static void AddNotificacionDependency(this IServiceCollection service)
        {
            service.AddScoped<INotificacionRepository, NotificacionRepository>();
            service.AddTransient<INotificacionService, NotificacionService>();
        }
    }
}