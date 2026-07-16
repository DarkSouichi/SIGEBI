using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SIGEBI.Application.Dtos.Penalties;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Penalties;
using SIGEBI.Persistence.Interfaces;

namespace SIGEBI.Application.Services
{
    public class PenalizacionService : IPenalizacionService
    {
        private readonly IPenalizacionRepository _penalizacionRepository;
        private readonly ILogger<PenalizacionService> _logger;
        private readonly IConfiguration _configuration;

        public PenalizacionService(IPenalizacionRepository penalizacionRepository,
                                   ILogger<PenalizacionService> logger,
                                   IConfiguration configuration)
        {
            _penalizacionRepository = penalizacionRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();
            try
            {
                result.Data = (await _penalizacionRepository.GetAllAsync())
                    .Select(p => new PenalizacionDto()
                    {
                        PenalizacionId = p.Id,
                        UsuarioId = p.UsuarioId,
                        PrestamoId = p.PrestamoId,
                        Monto = p.Monto,
                        Estado = p.Estado,
                        FechaEmision = p.FechaEmision,
                        ChangeDate = p.CreadoEn,
                        ChangeUser = p.Id
                    }).OrderByDescending(p => p.ChangeDate).ToList();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo las penalizaciones";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> GetById(int Id)
        {
            OperationResult result = new OperationResult();
            try
            {
                var penalizacion = await _penalizacionRepository.GetEntityByIdAsync(Id);
                result.Data = new PenalizacionDto()
                {
                    PenalizacionId = penalizacion.Id,
                    UsuarioId = penalizacion.UsuarioId,
                    PrestamoId = penalizacion.PrestamoId,
                    Monto = penalizacion.Monto,
                    Estado = penalizacion.Estado,
                    FechaEmision = penalizacion.FechaEmision,
                    ChangeDate = penalizacion.CreadoEn,
                    ChangeUser = penalizacion.Id
                };
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo la penalización";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> Save(SavePenalizacionDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await _penalizacionRepository.SaveEntityAsync(new Penalizacion()
                {
                    UsuarioId = dto.UsuarioId,
                    PrestamoId = dto.PrestamoId,
                    Monto = dto.Monto,
                    Estado = dto.Estado,
                    FechaEmision = dto.FechaEmision,
                    CreadoEn = dto.ChangeDate,
                    CreadoPor = dto.ChangeUser.ToString()
                });
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error guardando la penalización";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> Update(UpdatePenalizacionDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                var penalizacion = await _penalizacionRepository.GetEntityByIdAsync(dto.Id);
                penalizacion.Monto = dto.Monto;
                penalizacion.Estado = dto.Estado;
                penalizacion.ModificadoEn = dto.ChangeDate;
                penalizacion.ModificadoPor = dto.ChangeUser.ToString();
                await _penalizacionRepository.UpdateEntityAsync(penalizacion);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error actualizando la penalización";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public Task<OperationResult> Remove(RemovePenalizacionDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult> GetPenalizacionActivaByUsuarioId(int usuarioId)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await _penalizacionRepository.GetPenalizacionActivaByUsuarioId(usuarioId);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo la penalizacion activa del usuario";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> GetPenalizacionesByUsuarioId(int usuarioId)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await _penalizacionRepository.GetPenalizacionesByUsuarioId(usuarioId);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo las penalizaciones del usuario";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }
    }
}