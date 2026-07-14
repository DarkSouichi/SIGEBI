using Microsoft.Extensions.DependencyInjection;

namespace SIGEBI.IOC.Dependencies
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddUsuarioDependency();
            services.AddRecursoDependency();
            services.AddPrestamoDependency();
            services.AddPenalizacionDependency();
            services.AddNotificacionDependency();

            return services;
        }
    }
}