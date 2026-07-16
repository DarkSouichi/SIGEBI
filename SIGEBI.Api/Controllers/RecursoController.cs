using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Dtos.Catalog;
using SIGEBI.Application.Interfaces;

namespace SIGEBI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecursoController : ControllerBase
    {
        private readonly IRecursoService _recursoService;
        private readonly ILogger<RecursoController> _logger;

        public RecursoController(IRecursoService recursoService,
                                  ILogger<RecursoController> logger)
        {
            _recursoService = recursoService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _recursoService.GetAll();
            if (result.IsSuccess)
                return Ok(result);
            else
            {
                _logger.LogError("Error obteniendo recursos: {ErrorMessage}", result.Message);
                return BadRequest(result);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
                return BadRequest("El ID debe ser mayor a cero.");

            var result = await _recursoService.GetById(id);
            if (result.IsSuccess)
                return Ok(result);
            else
            {
                _logger.LogError("Error obteniendo recurso: {ErrorMessage}", result.Message);
                return BadRequest(result);
            }
        }

        [HttpPost("CrearRecurso")]
        public async Task<IActionResult> Post([FromBody] SaveRecursoDto dto)
        {
            var result = await _recursoService.Save(dto);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("ActualizarRecurso")]
        public async Task<IActionResult> Put([FromBody] UpdateRecursoDto dto)
        {
            var result = await _recursoService.Update(dto);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetByCategoria/{categoria}")]
        public async Task<IActionResult> GetByCategoria(string categoria)
        {
            if (string.IsNullOrEmpty(categoria))
                return BadRequest("La categoria no puede estar vacia.");

            var result = await _recursoService.GetRecursosByCategoria(categoria);
            if (result.IsSuccess)
                return Ok(result);
            else
            {
                _logger.LogError("Error obteniendo recursos por categoria: {ErrorMessage}", result.Message);
                return BadRequest(result);
            }
        }

        [HttpGet("GetDisponibles")]
        public async Task<IActionResult> GetDisponibles()
        {
            var result = await _recursoService.GetRecursosDisponibles();
            if (result.IsSuccess)
                return Ok(result);
            else
            {
                _logger.LogError("Error obteniendo recursos disponibles: {ErrorMessage}", result.Message);
                return BadRequest(result);
            }
        }

        [HttpGet("GetEjemplares/{recursoId}")]
        public async Task<IActionResult> GetEjemplares(int recursoId)
        {
            if (recursoId <= 0)
                return BadRequest("El ID debe ser mayor a cero.");

            var result = await _recursoService.GetEjemplaresByRecursoId(recursoId);
            if (result.IsSuccess)
                return Ok(result);
            else
            {
                _logger.LogError("Error obteniendo ejemplares: {ErrorMessage}", result.Message);
                return BadRequest(result);
            }
        }

    }
}