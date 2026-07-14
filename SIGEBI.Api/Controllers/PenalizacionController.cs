using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Dtos.Penalties;
using SIGEBI.Application.Interfaces;

namespace SIGEBI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PenalizacionController : ControllerBase
    {
        private readonly IPenalizacionService _penalizacionService;
        private readonly ILogger<PenalizacionController> _logger;

        public PenalizacionController(IPenalizacionService penalizacionService,
                                       ILogger<PenalizacionController> logger)
        {
            _penalizacionService = penalizacionService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _penalizacionService.GetAll();
            if (result.IsSuccess)
                return Ok(result);
            else
            {
                _logger.LogError("Error obteniendo penalizaciones: {ErrorMessage}", result.Message);
                return BadRequest(result);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
                return BadRequest("El ID debe ser mayor a cero.");

            var result = await _penalizacionService.GetById(id);
            if (result.IsSuccess)
                return Ok(result);
            else
            {
                _logger.LogError("Error obteniendo penalizacion: {ErrorMessage}", result.Message);
                return BadRequest(result);
            }
        }

        [HttpPost("CrearPenalizacion")]
        public async Task<IActionResult> Post([FromBody] SavePenalizacionDto dto)
        {
            var result = await _penalizacionService.Save(dto);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("ActualizarPenalizacion")]
        public async Task<IActionResult> Put([FromBody] UpdatePenalizacionDto dto)
        {
            var result = await _penalizacionService.Update(dto);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

    }
}