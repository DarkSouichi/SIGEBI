using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;
using SIGEBI.Persistence.Interfaces;
using SIGEBI.Persistence.Repositories.Catalog;

namespace SIGEBI.IOC.Dependencias
{
    public static class EjemplarDependency
    {
        public static void AddEjemplarDependency(this IServiceCollection services)
        {
            services.AddScoped<IEjemplarRepository, EjemplarRepository>();
            services.AddScoped<IEjemplarService, EjemplarService>();
        }
    }
}