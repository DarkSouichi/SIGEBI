using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Persistence.Interfaces;
using SIGEBI.Persistence.Repositories.Users;

namespace SIGEBI.IOC.Dependencias
{
    public static class RolDependency
    {
        public static void AddRolDependency(this IServiceCollection services)
        {
            services.AddScoped<IRolRepository, RolRepository>();
        }
    }
}