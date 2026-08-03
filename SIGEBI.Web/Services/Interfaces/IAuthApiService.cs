using SIGEBI.Web.Models.Auth;

namespace SIGEBI.Web.Services
{
    public interface IAuthApiService
    {
        Task<LoginResponseViewModel> Login(string email, string password);
    }
}