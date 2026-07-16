using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Dtos.Notifications;
using SIGEBI.Application.Interfaces;

namespace SIGEBI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacionController : ControllerBase
    {
        private readonly INotificacionService _notificacionService;
        private readonly ILogger<NotificacionController> _logger;

        public NotificacionController(INotificacionService notificacionService,
                                       ILogger<NotificacionController> logger)
        {
            _notificacionService = notificacionService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _notificacionService.GetAll();
            if (result.IsSuccess)
                return Ok(result);
            else
            {
                _logger.LogError("Error obteniendo notificaciones: {ErrorMessage}", result.Message);
                return BadRequest(result);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
                return BadRequest("El ID debe ser mayor a cero.");

            var result = await _notificacionService.GetById(id);
            if (result.IsSuccess)
                return Ok(result);
            else
            {
                _logger.LogError("Error obteniendo notificacion: {ErrorMessage}", result.Message);
                return BadRequest(result);
            }
        }

        [HttpPost("CrearNotificacion")]
        public async Task<IActionResult> Post([FromBody] SaveNotificacionDto dto)
        {
            var result = await _notificacionService.Save(dto);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("ActualizarNotificacion")]
        public async Task<IActionResult> Put([FromBody] UpdateNotificacionDto dto)
        {
            var result = await _notificacionService.Update(dto);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetByUsuario/{usuarioId}")]
        public async Task<IActionResult> GetByUsuario(int usuarioId)
        {
            if (usuarioId <= 0)
                return BadRequest("El ID debe ser mayor a cero.");

            var result = await _notificacionService.GetNotificacionesByUsuarioId(usuarioId);
            if (result.IsSuccess)
                return Ok(result);
            else
            {
                _logger.LogError("Error obteniendo notificaciones del usuario: {ErrorMessage}", result.Message);
                return BadRequest(result);
            }
        }

    }
}