using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;
using SIGEBI.Persistence.Interfaces;
using SIGEBI.Persistence.Repositories.Loans;

namespace SIGEBI.IOC.Dependencies
{
    public static class PrestamoDependency
    {
        public static void AddPrestamoDependency(this IServiceCollection service)
        {
            service.AddScoped<IPrestamoRepository, PrestamoRepository>();
            service.AddTransient<IPrestamoService, PrestamoService>();
        }
    }
}