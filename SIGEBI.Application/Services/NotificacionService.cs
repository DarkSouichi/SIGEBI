using Microsoft.Extensions.Configuration;
using SIGEBI.Application.Dtos.Notifications;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Notifications;
using SIGEBI.Infrastructure.Audit;  
using SIGEBI.Infrastructure.Logger;
using SIGEBI.Persistence.Interfaces;

namespace SIGEBI.Application.Services
{
    public class NotificacionService : INotificacionService
    {
        private readonly INotificacionRepository _notificacionRepository;
        private readonly ILoggerService<NotificacionService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAuditLogger _auditLogger;

        public NotificacionService(INotificacionRepository notificacionRepository,
                                   ILoggerService<NotificacionService> logger,
                                   IConfiguration configuration,
                                   IAuditLogger auditLogger) 
        {
            _notificacionRepository = notificacionRepository;
            _logger = logger;
            _configuration = configuration;
            _auditLogger = auditLogger;
        }

        public async Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();
            try
            {
                result.Data = (await _notificacionRepository.GetAllAsync())
                    .Select(n => new NotificacionDto()
                    {
                        NotificacionId = n.Id,
                        UsuarioId = n.UsuarioId,
                        Tipo = n.Tipo,
                        Mensaje = n.Mensaje,
                        Canal = n.Canal,
                        EnviadoEn = n.EnviadoEn,
                        ChangeDate = n.CreadoEn,
                        ChangeUser = n.Id
                    }).OrderByDescending(n => n.ChangeDate).ToList();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo las notificaciones";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> GetById(int Id)
        {
            OperationResult result = new OperationResult();
            try
            {
                var notificacion = await _notificacionRepository.GetEntityByIdAsync(Id);
                result.Data = new NotificacionDto()
                {
                    NotificacionId = notificacion.Id,
                    UsuarioId = notificacion.UsuarioId,
                    Tipo = notificacion.Tipo,
                    Mensaje = notificacion.Mensaje,
                    Canal = notificacion.Canal,
                    EnviadoEn = notificacion.EnviadoEn,
                    ChangeDate = notificacion.CreadoEn,
                    ChangeUser = notificacion.Id
                };
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo la notificación";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> Save(SaveNotificacionDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await _notificacionRepository.SaveEntityAsync(new Notificacion()
                {
                    UsuarioId = dto.UsuarioId,
                    Tipo = dto.Tipo,
                    Mensaje = dto.Mensaje,
                    Canal = dto.Canal,
                    EnviadoEn = DateTime.Now,
                    PrestamoId = dto.PrestamoId, 
                    RecursoId = dto.RecursoId,     
                    CreadoEn = dto.ChangeDate,
                    CreadoPor = dto.ChangeUser.ToString()
                });

                await _auditLogger.LogAsync(
                    actor: dto.ChangeUser.ToString(),
                    accion: "CrearNotificacion",
                    modulo: "Notificaciones",
                    resultado: result.IsSuccess ? "Exitoso" : "Fallido",
                    detalles: $"UsuarioId: {dto.UsuarioId}, Tipo: {dto.Tipo}"
                );
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error guardando la notificación";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> Update(UpdateNotificacionDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                var notificacion = await _notificacionRepository.GetEntityByIdAsync(dto.Id);
                notificacion.Tipo = dto.Tipo;
                notificacion.Mensaje = dto.Mensaje;
                notificacion.Canal = dto.Canal;
                notificacion.EnviadoEn = dto.EnviadoEn;
                notificacion.ModificadoEn = dto.ChangeDate;
                notificacion.ModificadoPor = dto.ChangeUser.ToString();
                await _notificacionRepository.UpdateEntityAsync(notificacion);

                result.Message = "Notificación actualizada correctamente";

                await _auditLogger.LogAsync(
                    actor: dto.ChangeUser.ToString(),
                    accion: "ActualizarNotificacion",
                    modulo: "Notificaciones",
                    resultado: "Exitoso",
                    detalles: $"Id: {dto.Id}, Tipo: {dto.Tipo}, Canal: {dto.Canal}"
                );
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error actualizando la notificación";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> GetNotificacionesByUsuarioId(int usuarioId)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await _notificacionRepository.GetNotificacionesByUsuarioId(usuarioId);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo las notificaciones del usuario";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }
    }
}