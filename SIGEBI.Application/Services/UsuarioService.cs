using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SIGEBI.Application.Dtos.Users;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Users;
using SIGEBI.Persistence.Interfaces;

namespace SIGEBI.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<UsuarioService> _logger;
        private readonly IConfiguration _configuration;

        public UsuarioService(IUsuarioRepository usuarioRepository,
                              ILogger<UsuarioService> logger,
                              IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();
            try
            {
                result.Data = (await _usuarioRepository.GetAllAsync())
                    .Select(u => new UsuarioDto()
                    {
                        UsuarioId = u.Id,
                        NombreCompleto = u.NombreCompleto,
                        Email = u.Email,
                        EstaActivo = u.EstaActivo,
                        RolId = u.RolId,
                        ChangeDate = u.CreadoEn,
                        ChangeUser = u.Id
                    }).OrderByDescending(u => u.ChangeDate).ToList();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo los usuarios";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> GetById(int Id)
        {
            OperationResult result = new OperationResult();
            try
            {
                var usuario = await _usuarioRepository.GetEntityByIdAsync(Id);
                result.Data = new UsuarioDto()
                {
                    UsuarioId = usuario.Id,
                    NombreCompleto = usuario.NombreCompleto,
                    Email = usuario.Email,
                    EstaActivo = usuario.EstaActivo,
                    RolId = usuario.RolId,
                    ChangeDate = usuario.CreadoEn,
                    ChangeUser = usuario.Id
                };
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo el usuario";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> Save(SaveUsuarioDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await _usuarioRepository.SaveEntityAsync(new Usuario()
                {
                    NombreCompleto = dto.NombreCompleto,
                    Email = dto.Email,
                    PasswordHash = dto.Password,
                    EstaActivo = dto.EstaActivo,
                    RolId = dto.RolId,
                    CreadoEn = dto.ChangeDate,
                    CreadoPor = dto.ChangeUser.ToString()
                });
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error guardando el usuario";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> Update(UpdateUsuarioDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                var usuario = await _usuarioRepository.GetEntityByIdAsync(dto.Id);
                usuario.NombreCompleto = dto.NombreCompleto;
                usuario.Email = dto.Email;
                usuario.EstaActivo = dto.EstaActivo;
                usuario.RolId = dto.RolId;
                usuario.ModificadoEn = dto.ChangeDate;
                usuario.ModificadoPor = dto.ChangeUser.ToString();
                await _usuarioRepository.UpdateEntityAsync(usuario);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error actualizando el usuario";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public Task<OperationResult> Remove(RemoveUsuarioDto dto)
        {
            throw new NotImplementedException();
        }
    }
}