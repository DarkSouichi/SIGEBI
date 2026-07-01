using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;
using SIGEBI.Persistence.Interfaces;
using SIGEBI.Persistence.Repositories.Catalog;

namespace SIGEBI.IOC.Dependencies
{
    public static class RecursoDependency
    {
        public static void AddRecursoDependency(this IServiceCollection service)
        {
            service.AddScoped<IRecursoRepository, RecursoRepository>();
            service.AddTransient<IRecursoService, RecursoService>();
        }
    }
}