using SIGEBI.Application.Dtos.Catalog;
using SIGEBI.Domain.Base;

namespace SIGEBI.Application.Interfaces
{
    public interface IEjemplarService
    {
        Task<OperationResult> GetAll();
        Task<OperationResult> GetById(int id);
        Task<OperationResult> Save(SaveEjemplarDto dto);
        Task<OperationResult> Update(UpdateEjemplarDto dto);
    }
}