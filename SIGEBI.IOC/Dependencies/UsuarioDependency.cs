using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;
using SIGEBI.Persistence.Interfaces;
using SIGEBI.Persistence.Repositories.Users;

namespace SIGEBI.IOC.Dependencies
{
    public static class UsuarioDependency
    {
        public static void AddUsuarioDependency(this IServiceCollection service)
        {
            service.AddScoped<IUsuarioRepository, UsuarioRepository>();
            service.AddTransient<IUsuarioService, UsuarioService>();
        }
    }
}