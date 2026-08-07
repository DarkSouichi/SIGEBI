using Microsoft.Extensions.Configuration;
using SIGEBI.Application.Dtos.Loans;
using SIGEBI.Application.Dtos.Notifications;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Catalog;
using SIGEBI.Domain.Entities.Loans;
using SIGEBI.Infrastructure.Audit;
using SIGEBI.Infrastructure.Logger;
using SIGEBI.Persistence.Interfaces;

namespace SIGEBI.Application.Services
{
    public class PrestamoService : IPrestamoService
    {
        private readonly IPrestamoRepository _prestamoRepository;
        private readonly IEjemplarRepository _ejemplarRepository;
        private readonly ILoggerService<PrestamoService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAuditLogger _auditLogger;
        private readonly INotificacionService _notificacionService;

        public PrestamoService(IPrestamoRepository prestamoRepository,
                               IEjemplarRepository ejemplarRepository,
                               ILoggerService<PrestamoService> logger,
                               IConfiguration configuration,
                               IAuditLogger auditLogger,
                               INotificacionService notificacionService) 
        {
            _prestamoRepository = prestamoRepository;
            _ejemplarRepository = ejemplarRepository;
            _logger = logger;
            _configuration = configuration;
            _auditLogger = auditLogger;
            _notificacionService = notificacionService; 
        }

        private async Task CrearNotificacion(int usuarioId, string tipo, string mensaje, int? prestamoId = null, int? recursoId = null)
        {
            var notificacionDto = new SaveNotificacionDto
            {
                UsuarioId = usuarioId,
                Tipo = tipo,
                Mensaje = mensaje,
                Canal = "Sistema",
                PrestamoId = prestamoId,
                RecursoId = recursoId,
                ChangeDate = DateTime.Now,
                ChangeUser = 1 
            };
            await _notificacionService.Save(notificacionDto);
        }

        public async Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();
            try
            {
                result.Data = (await _prestamoRepository.GetAllAsync())
                    .Select(p => new PrestamoDto()
                    {
                        PrestamoId = p.Id,
                        UsuarioId = p.UsuarioId,
                        EjemplarId = p.EjemplarId,
                        FechaPrestamo = p.FechaPrestamo,
                        FechaDevolucionEsperada = p.FechaDevolucionEsperada,
                        FechaDevolucionReal = p.FechaDevolucionReal,
                        Estado = p.Estado.ToString(),
                        ChangeDate = p.CreadoEn,
                        ChangeUser = p.Id
                    }).OrderByDescending(p => p.ChangeDate).ToList();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo los préstamos";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> GetById(int Id)
        {
            OperationResult result = new OperationResult();
            try
            {
                var prestamo = await _prestamoRepository.GetEntityByIdAsync(Id);

                if (prestamo == null)
                {
                    result.Success = false;
                    result.Message = "Préstamo no encontrado";
                    return result;
                }

                result.Data = new PrestamoDto()
                {
                    PrestamoId = prestamo.Id,
                    UsuarioId = prestamo.UsuarioId,
                    EjemplarId = prestamo.EjemplarId,
                    FechaPrestamo = prestamo.FechaPrestamo,
                    FechaDevolucionEsperada = prestamo.FechaDevolucionEsperada,
                    FechaDevolucionReal = prestamo.FechaDevolucionReal,
                    Estado = prestamo.Estado.ToString(),
                    ChangeDate = prestamo.CreadoEn,
                    ChangeUser = prestamo.Id
                };
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo el préstamo";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> Save(SavePrestamoDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                if (!Enum.TryParse<EstadoPrestamo>(dto.Estado, true, out var estadoEnum))
                {
                    result.Success = false;
                    result.Message = "Estado de préstamo inválido. Valores permitidos: Pendiente, Activo, Devuelto, Vencido";
                    return result;
                }

                var prestamo = new Prestamo()
                {
                    UsuarioId = dto.UsuarioId,
                    EjemplarId = dto.EjemplarId,
                    FechaPrestamo = dto.FechaPrestamo,
                    FechaDevolucionEsperada = dto.FechaDevolucionEsperada,
                    FechaDevolucionReal = dto.FechaDevolucionReal,
                    Estado = estadoEnum,
                    CreadoEn = dto.ChangeDate,
                    CreadoPor = dto.ChangeUser.ToString()
                };

                result = await _prestamoRepository.SaveEntityAsync(prestamo);

                if (result.IsSuccess)
                {
                    int prestamoId = (result.Data as Prestamo)?.Id ?? 0;

                    if (estadoEnum == EstadoPrestamo.Pendiente)
                    {
                        await CrearNotificacion(
                            dto.UsuarioId,
                            "Solicitud de Préstamo",
                            $"Has solicitado el préstamo del ejemplar #{dto.EjemplarId}. Espera la aprobación.",
                            prestamoId: prestamoId
                        );
                    }
                    else if (estadoEnum == EstadoPrestamo.Activo)
                    {
                        await CrearNotificacion(
                            dto.UsuarioId,
                            "Préstamo Aprobado",
                            $"Tu préstamo del ejemplar #{dto.EjemplarId} ha sido aprobado.",
                            prestamoId: prestamoId
                        );

                        var ejemplar = await _ejemplarRepository.GetEntityByIdAsync(dto.EjemplarId);
                        if (ejemplar != null)
                        {
                            ejemplar.Estado = EstadoEjemplar.Prestado;
                            ejemplar.ModificadoEn = DateTime.Now;
                            ejemplar.ModificadoPor = dto.ChangeUser.ToString();
                            await _ejemplarRepository.UpdateEntityAsync(ejemplar);
                        }
                    }
                }

                await _auditLogger.LogAsync(
                    actor: dto.ChangeUser.ToString(),
                    accion: "CrearPrestamo",
                    modulo: "Préstamos",
                    resultado: result.IsSuccess ? "Exitoso" : "Fallido",
                    detalles: $"UsuarioId: {dto.UsuarioId}, EjemplarId: {dto.EjemplarId}, Estado: {dto.Estado}"
                );
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error guardando el préstamo";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> Update(UpdatePrestamoDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                var prestamo = await _prestamoRepository.GetEntityByIdAsync(dto.Id);
                if (prestamo == null)
                {
                    result.Success = false;
                    result.Message = "Préstamo no encontrado";
                    return result;
                }

                if (!Enum.TryParse<EstadoPrestamo>(dto.Estado, true, out var estadoEnum))
                {
                    result.Success = false;
                    result.Message = "Estado de préstamo inválido";
                    return result;
                }

                var estadoAnterior = prestamo.Estado;

                prestamo.FechaDevolucionEsperada = dto.FechaDevolucionEsperada;
                prestamo.FechaDevolucionReal = dto.FechaDevolucionReal;
                prestamo.Estado = estadoEnum;
                prestamo.ModificadoEn = dto.ChangeDate;
                prestamo.ModificadoPor = dto.ChangeUser.ToString();

                await _prestamoRepository.UpdateEntityAsync(prestamo);

                if (estadoAnterior == EstadoPrestamo.Pendiente && estadoEnum == EstadoPrestamo.Activo)
                {
                    await CrearNotificacion(
                        prestamo.UsuarioId,
                        "Préstamo Aprobado",
                        $"Tu préstamo del ejemplar #{prestamo.EjemplarId} ha sido aprobado por el administrador.",
                        prestamoId: prestamo.Id
                    );

                    var ejemplar = await _ejemplarRepository.GetEntityByIdAsync(prestamo.EjemplarId);
                    if (ejemplar != null)
                    {
                        ejemplar.Estado = EstadoEjemplar.Prestado;
                        ejemplar.ModificadoEn = DateTime.Now;
                        ejemplar.ModificadoPor = dto.ChangeUser.ToString();
                        await _ejemplarRepository.UpdateEntityAsync(ejemplar);
                    }
                }

                if (estadoAnterior == EstadoPrestamo.Activo && estadoEnum == EstadoPrestamo.Devuelto)
                {
                    await CrearNotificacion(
                        prestamo.UsuarioId,
                        "Devolución Registrada",
                        $"Tu devolución del ejemplar #{prestamo.EjemplarId} ha sido registrada.",
                        prestamoId: prestamo.Id
                    );

                    var ejemplar = await _ejemplarRepository.GetEntityByIdAsync(prestamo.EjemplarId);
                    if (ejemplar != null)
                    {
                        ejemplar.Estado = EstadoEjemplar.Disponible;
                        ejemplar.ModificadoEn = DateTime.Now;
                        ejemplar.ModificadoPor = dto.ChangeUser.ToString();
                        await _ejemplarRepository.UpdateEntityAsync(ejemplar);
                    }
                }

                result.Message = "Préstamo actualizado correctamente";

                await _auditLogger.LogAsync(
                    actor: dto.ChangeUser.ToString(),
                    accion: "ActualizarPrestamo",
                    modulo: "Préstamos",
                    resultado: "Exitoso",
                    detalles: $"Id: {dto.Id}, Estado: {dto.Estado}"
                );
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error actualizando el préstamo";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> GetPrestamosByUsuarioId(int usuarioId)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await _prestamoRepository.GetPrestamosByUsuarioId(usuarioId);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo los prestamos del usuario";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> GetPrestamosActivos()
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await _prestamoRepository.GetPrestamosActivos();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo los prestamos activos";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> GetPrestamosByEjemplarId(int ejemplarId)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await _prestamoRepository.GetPrestamosByEjemplarId(ejemplarId);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo los prestamos del ejemplar";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }
    }
}