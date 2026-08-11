using Microsoft.EntityFrameworkCore;
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
    public class RecursoService : IRecursoService
    {
        private readonly IRecursoRepository _recursoRepository;
        private readonly ILoggerService<RecursoService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAuditLogger _auditLogger;

        public RecursoService(IRecursoRepository recursoRepository,
                              ILoggerService<RecursoService> logger,
                              IConfiguration configuration,
                              IAuditLogger auditLogger)
        {
            _recursoRepository = recursoRepository;
            _logger = logger;
            _configuration = configuration;
            _auditLogger = auditLogger;
        }

        public async Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();
            try
            {
                var recursos = await _recursoRepository.GetAllWithEjemplaresAsync();

                result.Data = recursos.Select(r => new RecursoDto()
                {
                    RecursoId = r.Id,
                    Titulo = r.Titulo,
                    Autor = r.Autor,
                    ISBN = r.ISBN,
                    Categoria = r.Categoria,
                    ChangeDate = r.CreadoEn,
                    ChangeUser = r.Id,
                    TotalEjemplares = r.Ejemplares?.Count ?? 0,
                    EjemplaresDisponibles = r.Ejemplares?.Count(e => e.Estado == EstadoEjemplar.Disponible) ?? 0,
                               Descripcion = r.Descripcion,
                    FechaLanzamiento = r.FechaLanzamiento
                }).OrderByDescending(r => r.ChangeDate).ToList();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo los recursos";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> GetById(int Id)
        {
            OperationResult result = new OperationResult();
            try
            {
                var recurso = await _recursoRepository.GetByIdWithEjemplaresAsync(Id);
                if (recurso == null)
                {
                    result.Success = false;
                    result.Message = "Recurso no encontrado";
                    return result;
                }

                result.Data = new RecursoDto()
                {
                    RecursoId = recurso.Id,
                    Titulo = recurso.Titulo,
                    Autor = recurso.Autor,
                    ISBN = recurso.ISBN,
                    Categoria = recurso.Categoria,
                    ChangeDate = recurso.CreadoEn,
                    ChangeUser = recurso.Id,
                    TotalEjemplares = recurso.Ejemplares?.Count ?? 0,
                    EjemplaresDisponibles = recurso.Ejemplares?.Count(e => e.Estado == EstadoEjemplar.Disponible) ?? 0,
                    Descripcion = recurso.Descripcion,
                    FechaLanzamiento = recurso.FechaLanzamiento
                };
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo el recurso";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> Save(SaveRecursoDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await _recursoRepository.SaveEntityAsync(new Recurso()
                {
                    Titulo = dto.Titulo,
                    Autor = dto.Autor,
                    ISBN = dto.ISBN,
                    Categoria = dto.Categoria,
                    Descripcion = dto.Descripcion,
                    FechaLanzamiento = dto.FechaLanzamiento,
                    CreadoEn = dto.ChangeDate,
                    CreadoPor = dto.ChangeUser.ToString()
                });

                await _auditLogger.LogAsync(
                    actor: dto.ChangeUser.ToString(),
                    accion: "CrearRecurso",
                    modulo: "Recursos",
                    resultado: result.IsSuccess ? "Exitoso" : "Fallido",
                    detalles: $"Titulo: {dto.Titulo}, Autor: {dto.Autor}, ISBN: {dto.ISBN}"
                );
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error guardando el recurso";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> Update(UpdateRecursoDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                var recurso = await _recursoRepository.GetEntityByIdAsync(dto.Id);
                if (recurso == null)
                {
                    result.Success = false;
                    result.Message = "Recurso no encontrado";
                    return result;
                }

                recurso.Titulo = dto.Titulo;
                recurso.Autor = dto.Autor;
                recurso.ISBN = dto.ISBN;
                recurso.Categoria = dto.Categoria;
                recurso.Descripcion = dto.Descripcion;
                recurso.FechaLanzamiento = dto.FechaLanzamiento;
                recurso.ModificadoEn = dto.ChangeDate;
                recurso.ModificadoPor = dto.ChangeUser.ToString();
                await _recursoRepository.UpdateEntityAsync(recurso);

                result.Message = "Recurso actualizado correctamente";

                await _auditLogger.LogAsync(
                    actor: dto.ChangeUser.ToString(),
                    accion: "ActualizarRecurso",
                    modulo: "Recursos",
                    resultado: "Exitoso",
                    detalles: $"Id: {dto.Id}, Titulo: {dto.Titulo}"
                );
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error actualizando el recurso";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> GetEjemplaresByRecursoId(int recursoId)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await _recursoRepository.GetEjemplaresByRecursoId(recursoId);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo los ejemplares del recurso";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> GetRecursosByCategoria(string categoria)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await _recursoRepository.GetRecursosByCategoria(categoria);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo los recursos por categoria";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> GetRecursosDisponibles()
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await _recursoRepository.GetRecursosDisponibles();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo los recursos disponibles";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }
    }
}