using Microsoft.Extensions.DependencyInjection;
using SIGEBI.IOC.Dependencies;

namespace SIGEBI.IOC
{
    public static class DependencyContainer
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddUsuarioDependency();
            services.AddRecursoDependency();
            services.AddPrestamoDependency();
            services.AddPenalizacionDependency();
            services.AddNotificacionDependency();
        }
    }
}