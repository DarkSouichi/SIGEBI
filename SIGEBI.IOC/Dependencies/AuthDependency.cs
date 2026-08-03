using Microsoft.Extensions.DependencyInjection;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Services;

namespace SIGEBI.IOC.Dependencias
{
    public static class AuthDependency
    {
        public static void AddAuthDependency(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
        }
    }
}