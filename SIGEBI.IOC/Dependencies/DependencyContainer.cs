using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Infrastructure.Audit;
using SIGEBI.Infrastructure.Logger;
using SIGEBI.IOC.Dependencias;
using SIGEBI.IOC.Dependencies;

namespace SIGEBI.IOC.Dependencias
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddScoped(typeof(ILoggerService<>), typeof(LoggerService<>));
            services.AddScoped<IAuditLogger, AuditLogger>();
            services.AddAuthDependency();
            services.AddRolDependency();
            services.AddUsuarioDependency();
            services.AddRecursoDependency();
            services.AddPrestamoDependency();
            services.AddPenalizacionDependency();
            services.AddNotificacionDependency();
            services.AddEjemplarDependency();
            return services;
        }
    }
}