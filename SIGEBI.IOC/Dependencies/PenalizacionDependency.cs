using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;
using SIGEBI.Persistence.Interfaces;
using SIGEBI.Persistence.Repositories.Penalties;

namespace SIGEBI.IOC.Dependencies
{
    public static class PenalizacionDependency
    {
        public static void AddPenalizacionDependency(this IServiceCollection service)
        {
            service.AddScoped<IPenalizacionRepository, PenalizacionRepository>();
            service.AddTransient<IPenalizacionService, PenalizacionService>();
        }
    }
}