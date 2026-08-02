using SIGEBI.Application.Dtos.Auth;
using SIGEBI.Domain.Base;

namespace SIGEBI.Application.Interfaces
{
    public interface IAuthService
    {
        Task<OperationResult> Login(LoginDto dto);
    }
}