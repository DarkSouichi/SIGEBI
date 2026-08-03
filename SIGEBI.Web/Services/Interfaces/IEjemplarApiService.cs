using SIGEBI.Web.Models;
using SIGEBI.Web.Models.Ejemplar;

namespace SIGEBI.Web.Services
{
    public interface IEjemplarApiService
    {
        Task<GetAllEjemplaresResponse> GetAll();
        Task<GetEjemplarResponse> GetById(int id);
        Task<ApiResponse> Create(EjemplarCreateModel model);
        Task<ApiResponse> Update(EjemplarEditModel model);
    }
}