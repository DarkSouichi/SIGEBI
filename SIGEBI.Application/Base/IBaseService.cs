using SIGEBI.Domain.Base;

namespace SIGEBI.Application.Base
{
    public interface IBaseService<TDtoSave, TDtoUpdate>
    {
        Task<OperationResult> GetAll();
        Task<OperationResult> GetById(int Id);
        Task<OperationResult> Save(TDtoSave dto);
        Task<OperationResult> Update(TDtoUpdate dto);
    }
}