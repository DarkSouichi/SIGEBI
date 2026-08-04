using SIGEBI.Web.Models;
using SIGEBI.Web.Models.Usuario;

namespace SIGEBI.Web.Services
{
    public interface IUsuarioApiService
    {
        Task<GetAllUsuariosResponse> GetAll();
        Task<GetUsuarioResponse> GetById(int id);
        Task<ApiResponse> Create(UsuarioCreateModel model);
        Task<ApiResponse> Update(UsuarioEditModel model);
    }
}