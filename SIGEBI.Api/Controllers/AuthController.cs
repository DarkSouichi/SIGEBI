using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Dtos.Auth;
using SIGEBI.Application.Interfaces;
using SIGEBI.Infrastructure.Logger;

namespace SIGEBI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILoggerService<AuthController> _logger;

        public AuthController(IAuthService authService,
                               ILoggerService<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [AllowAnonymous]  
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.Register(dto);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
                return BadRequest(new { message = "El email y la contraseña son requeridos." });

            var result = await _authService.Login(dto);
            if (result.IsSuccess)
                return Ok(result);
            else
            {
                _logger.LogError($"Error en login: {result.Message}", new Exception(result.Message));
                return Unauthorized(result);
            }
        }
    }
}