using SIGEBI.Web.Models;
using SIGEBI.Web.Models.Prestamo;

namespace SIGEBI.Web.Services
{
    public interface IPrestamoApiService
    {
        Task<GetAllPrestamosResponse> GetAll();
        Task<GetPrestamoResponse> GetById(int id);
        Task<ApiResponse> Create(PrestamoCreateModel model);
        Task<ApiResponse> Update(PrestamoEditModel model);
    }
}