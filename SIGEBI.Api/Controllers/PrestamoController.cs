using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Dtos.Loans;
using SIGEBI.Application.Interfaces;

namespace SIGEBI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamoController : ControllerBase
    {
        private readonly IPrestamoService _prestamoService;
        private readonly ILogger<PrestamoController> _logger;

        public PrestamoController(IPrestamoService prestamoService,
                                   ILogger<PrestamoController> logger)
        {
            _prestamoService = prestamoService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _prestamoService.GetAll();
            if (result.IsSuccess)
                return Ok(result);
            else
            {
                _logger.LogError("Error obteniendo prestamos: {ErrorMessage}", result.Message);
                return BadRequest(result);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
                return BadRequest("El ID debe ser mayor a cero.");

            var result = await _prestamoService.GetById(id);
            if (result.IsSuccess)
                return Ok(result);
            else
            {
                _logger.LogError("Error obteniendo prestamo: {ErrorMessage}", result.Message);
                return BadRequest(result);
            }
        }

        [HttpPost("CrearPrestamo")]
        public async Task<IActionResult> Post([FromBody] SavePrestamoDto dto)
        {
            var result = await _prestamoService.Save(dto);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("ActualizarPrestamo")]
        public async Task<IActionResult> Put([FromBody] UpdatePrestamoDto dto)
        {
            var result = await _prestamoService.Update(dto);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

    }
}