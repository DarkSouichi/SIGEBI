using Microsoft.Extensions.Configuration;
using SIGEBI.Application.Dtos.Users;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Users;
using SIGEBI.Infrastructure.Audit; 
using SIGEBI.Infrastructure.Logger;
using SIGEBI.Persistence.Interfaces;
using BCrypt.Net;

namespace SIGEBI.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILoggerService<UsuarioService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAuditLogger _auditLogger; 

        public UsuarioService(IUsuarioRepository usuarioRepository,
                              ILoggerService<UsuarioService> logger,
                              IConfiguration configuration,
                              IAuditLogger auditLogger) 
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
            _configuration = configuration;
            _auditLogger = auditLogger;
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
                if (usuario == null)
                {
                    result.Success = false;
                    result.Message = "Usuario no encontrado";
                    return result;
                }

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
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                result = await _usuarioRepository.SaveEntityAsync(new Usuario()
                {
                    NombreCompleto = dto.NombreCompleto,
                    Email = dto.Email,
                    PasswordHash = passwordHash,
                    EstaActivo = dto.EstaActivo,
                    RolId = dto.RolId,
                    CreadoEn = dto.ChangeDate,
                    CreadoPor = dto.ChangeUser.ToString()
                });

                await _auditLogger.LogAsync(
                    actor: dto.ChangeUser.ToString(),
                    accion: "CrearUsuario",
                    modulo: "Usuarios",
                    resultado: result.IsSuccess ? "Exitoso" : "Fallido",
                    detalles: $"Email: {dto.Email}, Nombre: {dto.NombreCompleto}"
                );
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

                if (usuario == null)
                {
                    result.Success = false;
                    result.Message = "Usuario no encontrado.";
                    return result;
                }

                usuario.NombreCompleto = dto.NombreCompleto;
                usuario.Email = dto.Email;
                usuario.EstaActivo = dto.EstaActivo;
                usuario.RolId = dto.RolId;
                usuario.ModificadoEn = dto.ChangeDate;
                usuario.ModificadoPor = dto.ChangeUser.ToString();

                await _usuarioRepository.UpdateEntityAsync(usuario);

                result.Message = "Usuario actualizado correctamente";

                await _auditLogger.LogAsync(
                    actor: dto.ChangeUser.ToString(),
                    accion: "ActualizarUsuario",
                    modulo: "Usuarios",
                    resultado: "Exitoso",
                    detalles: $"Id: {dto.Id}, Email: {dto.Email}"
                );
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error actualizando el usuario";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> GetUsuarioByEmail(string email)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await _usuarioRepository.GetUsuarioByEmail(email);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo el usuario por email";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> GetUsuariosActivos()
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await _usuarioRepository.GetUsuariosActivos();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo los usuarios activos";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> VerificarHabilitacion(int usuarioId)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await _usuarioRepository.VerificarHabilitacion(usuarioId);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error verificando la habilitacion del usuario";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }
    }
}