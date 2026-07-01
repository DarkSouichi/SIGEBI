using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SIGEBI.Application.Dtos.Catalog;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Catalog;
using SIGEBI.Persistence.Interfaces;

namespace SIGEBI.Application.Services
{
    public class RecursoService : IRecursoService
    {
        private readonly IRecursoRepository _recursoRepository;
        private readonly ILogger<RecursoService> _logger;
        private readonly IConfiguration _configuration;

        public RecursoService(IRecursoRepository recursoRepository,
                              ILogger<RecursoService> logger,
                              IConfiguration configuration)
        {
            _recursoRepository = recursoRepository;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();
            try
            {
                result.Data = (await _recursoRepository.GetAllAsync())
                    .Select(r => new RecursoDto()
                    {
                        RecursoId = r.Id,
                        Titulo = r.Titulo,
                        Autor = r.Autor,
                        ISBN = r.ISBN,
                        Categoria = r.Categoria,
                        ChangeDate = r.CreadoEn,
                        ChangeUser = r.Id
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
                var recurso = await _recursoRepository.GetEntityByIdAsync(Id);
                result.Data = new RecursoDto()
                {
                    RecursoId = recurso.Id,
                    Titulo = recurso.Titulo,
                    Autor = recurso.Autor,
                    ISBN = recurso.ISBN,
                    Categoria = recurso.Categoria,
                    ChangeDate = recurso.CreadoEn,
                    ChangeUser = recurso.Id
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
                    CreadoEn = dto.ChangeDate,
                    CreadoPor = dto.ChangeUser.ToString()
                });
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
                recurso.Titulo = dto.Titulo;
                recurso.Autor = dto.Autor;
                recurso.ISBN = dto.ISBN;
                recurso.Categoria = dto.Categoria;
                recurso.ModificadoEn = dto.ChangeDate;
                recurso.ModificadoPor = dto.ChangeUser.ToString();
                await _recursoRepository.UpdateEntityAsync(recurso);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error actualizando el recurso";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public Task<OperationResult> Remove(RemoveRecursoDto dto)
        {
            throw new NotImplementedException();
        }
    }
}