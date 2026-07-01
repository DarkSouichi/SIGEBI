using SIGEBI.Application.Base;
using SIGEBI.Application.Dtos.Users;

namespace SIGEBI.Application.Interfaces
{
    public interface IUsuarioService : IBaseService<SaveUsuarioDto, UpdateUsuarioDto, RemoveUsuarioDto>
    {
    }
}