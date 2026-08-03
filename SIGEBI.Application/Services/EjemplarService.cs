using Microsoft.Extensions.Configuration;
using SIGEBI.Application.Dtos.Catalog;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Catalog;
using SIGEBI.Infrastructure.Audit;
using SIGEBI.Infrastructure.Logger;
using SIGEBI.Persistence.Interfaces;

namespace SIGEBI.Application.Services
{
    public class EjemplarService : IEjemplarService
    {
        private readonly IEjemplarRepository _ejemplarRepository;
        private readonly ILoggerService<EjemplarService> _logger;
        private readonly IAuditLogger _auditLogger;
        private readonly IConfiguration _configuration;

        public EjemplarService(IEjemplarRepository ejemplarRepository,
                               ILoggerService<EjemplarService> logger,
                               IAuditLogger auditLogger,
                               IConfiguration configuration)
        {
            _ejemplarRepository = ejemplarRepository;
            _logger = logger;
            _auditLogger = auditLogger;
            _configuration = configuration;
        }

        public async Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();
            try
            {
                result.Data = await _ejemplarRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo ejemplares";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> GetById(int id)
        {
            OperationResult result = new OperationResult();
            try
            {
                result.Data = await _ejemplarRepository.GetEntityByIdAsync(id);
                if (result.Data == null)
                {
                    result.Success = false;
                    result.Message = "Ejemplar no encontrado";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo el ejemplar";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> Save(SaveEjemplarDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                var ejemplar = new Ejemplar
                {
                    CodigoBarras = dto.CodigoBarras,
                    Estado = (EstadoEjemplar)dto.Estado,
                    RecursoId = dto.RecursoId,
                    CreadoEn = dto.ChangeDate,
                    CreadoPor = dto.ChangeUser.ToString()
                };

                result = await _ejemplarRepository.SaveEntityAsync(ejemplar);

                await _auditLogger.LogAsync(
                    actor: dto.ChangeUser.ToString(),
                    accion: "CrearEjemplar",
                    modulo: "Ejemplares",
                    resultado: result.IsSuccess ? "Exitoso" : "Fallido",
                    detalles: $"Código: {dto.CodigoBarras}, RecursoId: {dto.RecursoId}"
                );
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error guardando el ejemplar";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> Update(UpdateEjemplarDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                var ejemplar = await _ejemplarRepository.GetEntityByIdAsync(dto.Id);
                if (ejemplar == null)
                {
                    result.Success = false;
                    result.Message = "Ejemplar no encontrado";
                    return result;
                }

                ejemplar.CodigoBarras = dto.CodigoBarras;
                ejemplar.Estado = (EstadoEjemplar)dto.Estado;
                ejemplar.RecursoId = dto.RecursoId;
                ejemplar.ModificadoEn = dto.ChangeDate;
                ejemplar.ModificadoPor = dto.ChangeUser.ToString();

                await _ejemplarRepository.UpdateEntityAsync(ejemplar);
                result.Message = "Ejemplar actualizado correctamente";

                await _auditLogger.LogAsync(
                    actor: dto.ChangeUser.ToString(),
                    accion: "ActualizarEjemplar",
                    modulo: "Ejemplares",
                    resultado: "Exitoso",
                    detalles: $"Id: {dto.Id}, Código: {dto.CodigoBarras}"
                );
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error actualizando el ejemplar";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }
    }
}