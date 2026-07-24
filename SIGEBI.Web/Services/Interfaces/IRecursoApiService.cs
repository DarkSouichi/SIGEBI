using SIGEBI.Web.Models.Recurso;

namespace SIGEBI.Web.Services
{
    public interface IRecursoApiService
    {
        Task<GetAllRecursosResponse> GetAll();
        Task<GetRecursoResponse> GetById(int id);
        Task<ApiResponse> Create(RecursoCreateModel model);
        Task<ApiResponse> Update(RecursoEditModel model);
    }
}