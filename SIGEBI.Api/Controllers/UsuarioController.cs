using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Dtos.Users;
using SIGEBI.Application.Interfaces;

namespace SIGEBI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(IUsuarioService usuarioService,
                                  ILogger<UsuarioController> logger)
        {
            _usuarioService = usuarioService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _usuarioService.GetAll();
            if (result.IsSuccess)
                return Ok(result);
            else
            {
                _logger.LogError("Error obteniendo usuarios: {ErrorMessage}", result.Message);
                return BadRequest(result);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
                return BadRequest("El ID debe ser mayor a cero.");

            var result = await _usuarioService.GetById(id);
            if (result.IsSuccess)
                return Ok(result);
            else
            {
                _logger.LogError("Error obteniendo usuario: {ErrorMessage}", result.Message);
                return BadRequest(result);
            }
        }

        [HttpGet("GetByEmail/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("El email no puede estar vacio.");

            var result = await _usuarioService.GetUsuarioByEmail(email);
            if (result.IsSuccess)
                return Ok(result);
            else
            {
                _logger.LogError("Error obteniendo usuario por email: {ErrorMessage}", result.Message);
                return BadRequest(result);
            }
        }

        [HttpGet("GetActivos")]
        public async Task<IActionResult> GetActivos()
        {
            var result = await _usuarioService.GetUsuariosActivos();
            if (result.IsSuccess)
                return Ok(result);
            else
            {
                _logger.LogError("Error obteniendo usuarios activos: {ErrorMessage}", result.Message);
                return BadRequest(result);
            }
        }

        [HttpGet("VerificarHabilitacion/{id}")]
        public async Task<IActionResult> VerificarHabilitacion(int id)
        {
            if (id <= 0)
                return BadRequest("El ID debe ser mayor a cero.");

            var result = await _usuarioService.VerificarHabilitacion(id);
            if (result.IsSuccess)
                return Ok(result);
            else
            {
                _logger.LogError("Error verificando habilitacion: {ErrorMessage}", result.Message);
                return BadRequest(result);
            }
        }

        [HttpPost("CrearUsuario")]
        public async Task<IActionResult> Post([FromBody] SaveUsuarioDto dto)
        {
            var result = await _usuarioService.Save(dto);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("ActualizarUsuario")]
        public async Task<IActionResult> Put([FromBody] UpdateUsuarioDto dto)
        {
            var result = await _usuarioService.Update(dto);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

    }
}