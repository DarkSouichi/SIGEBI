using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SIGEBI.Application.Dtos.Loans;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Loans;
using SIGEBI.Persistence.Interfaces;

namespace SIGEBI.Application.Services
{
    public class PrestamoService : IPrestamoService
    {
        private readonly IPrestamoRepository _prestamoRepository;
        private readonly ILogger<PrestamoService> _logger;
        private readonly IConfiguration _configuration;

        public PrestamoService(IPrestamoRepository prestamoRepository,
                               ILogger<PrestamoService> logger,
                               IConfiguration configuration)
        {
            _prestamoRepository = prestamoRepository;
            _logger = logger;
            _configuration = configuration;
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

                prestamo.FechaDevolucionEsperada = dto.FechaDevolucionEsperada;
                prestamo.FechaDevolucionReal = dto.FechaDevolucionReal;
                prestamo.Estado = estadoEnum;
                prestamo.ModificadoEn = dto.ChangeDate;
                prestamo.ModificadoPor = dto.ChangeUser.ToString();

                await _prestamoRepository.UpdateEntityAsync(prestamo);
                result.Message = "Préstamo actualizado correctamente";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error actualizando el préstamo";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public Task<OperationResult> Remove(RemovePrestamoDto dto)
        {
            throw new NotImplementedException();
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