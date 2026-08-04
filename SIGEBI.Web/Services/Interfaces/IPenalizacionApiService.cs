using SIGEBI.Web.Models;
using SIGEBI.Web.Models.Penalizacion;

namespace SIGEBI.Web.Services
{
    public interface IPenalizacionApiService
    {
        Task<GetAllPenalizacionesResponse> GetAll();
        Task<GetPenalizacionResponse> GetById(int id);
        Task<ApiResponse> Create(PenalizacionCreateModel model);
        Task<ApiResponse> Update(PenalizacionEditModel model);
    }
}