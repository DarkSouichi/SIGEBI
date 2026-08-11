using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SIGEBI.Application.Dtos.Auth;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Users;
using SIGEBI.Infrastructure.Audit;
using SIGEBI.Infrastructure.Logger;
using SIGEBI.Persistence.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SIGEBI.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IRolRepository _rolRepository; 
        private readonly ILoggerService<AuthService> _logger;
        private readonly IAuditLogger _auditLogger;  
        private readonly IConfiguration _configuration;

        public AuthService(IUsuarioRepository usuarioRepository,
                           IRolRepository rolRepository, 
                           ILoggerService<AuthService> logger,
                           IAuditLogger auditLogger,
                           IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _rolRepository = rolRepository;
            _logger = logger;
            _auditLogger = auditLogger;
            _configuration = configuration;
        }

        public async Task<OperationResult> Register(RegisterDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                var existingUser = await _usuarioRepository.GetUsuarioByEmail(dto.Email);
                if (existingUser.IsSuccess && existingUser.Data != null)
                {
                    result.Success = false;
                    result.Message = "El correo electrónico ya está registrado.";
                    return result;
                }

                var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                var nuevoUsuario = new Usuario
                {
                    NombreCompleto = dto.NombreCompleto,
                    Email = dto.Email,
                    PasswordHash = passwordHash,
                    EstaActivo = true,
                    RolId = 2,
                    CreadoEn = DateTime.Now,
                    CreadoPor = "Sistema"
                };

                result = await _usuarioRepository.SaveEntityAsync(nuevoUsuario);

                await _auditLogger.LogAsync(
                    actor: "Sistema",
                    accion: "RegistroUsuario",
                    modulo: "Auth",
                    resultado: result.IsSuccess ? "Exitoso" : "Fallido",
                    detalles: $"Email: {dto.Email}"
                );
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al registrar el usuario";
                _logger.LogError(result.Message, ex);
            }
            return result;
        }

        public async Task<OperationResult> Login(LoginDto dto)
        {
            OperationResult result = new OperationResult();
            try
            {
                var usuarioResult = await _usuarioRepository.GetUsuarioByEmail(dto.Email);

                if (!usuarioResult.IsSuccess || usuarioResult.Data == null)
                {
                    result.Success = false;
                    result.Message = "Usuario o contraseña incorrectos.";
                    await _auditLogger.LogAsync("Sistema", "LoginFallido", "Auth", "Fallido", $"Email: {dto.Email}");
                    return result;
                }

                var usuario = usuarioResult.Data as Domain.Entities.Users.Usuario;

                if (usuario == null || !usuario.EstaActivo)
                {
                    result.Success = false;
                    result.Message = "El usuario no está activo.";
                    await _auditLogger.LogAsync("Sistema", "LoginFallido", "Auth", "Fallido", $"Email: {dto.Email} - Inactivo");
                    return result;
                }

                if (!BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
                {
                    result.Success = false;
                    result.Message = "Usuario o contraseña incorrectos.";
                    await _auditLogger.LogAsync("Sistema", "LoginFallido", "Auth", "Fallido", $"Email: {dto.Email} - Contraseña incorrecta");
                    return result;
                }

                var rol = await _rolRepository.GetEntityByIdAsync(usuario.RolId);
                string rolNombre = rol?.Nombre ?? "Usuario";

                var token = GenerarToken(usuario, rolNombre);

                result.Data = new LoginResponseDto
                {
                    Token = token,
                    NombreCompleto = usuario.NombreCompleto,
                    Email = usuario.Email,
                    Rol = rolNombre,
                    UsuarioId = usuario.Id,
                    Expiracion = DateTime.Now.AddMinutes(
                        int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "60"))
                };

                await _auditLogger.LogAsync(
                    actor: usuario.Id.ToString(),
                    accion: "LoginExitoso",
                    modulo: "Auth",
                    resultado: "Exitoso",
                    detalles: $"Email: {dto.Email}"
                );
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error al iniciar sesión.";
                _logger.LogError(result.Message, ex);
                await _auditLogger.LogAsync("Sistema", "LoginError", "Auth", "Fallido", $"Email: {dto.Email} - {ex.Message}");
            }
            return result;
        }

        private string GenerarToken(Domain.Entities.Users.Usuario usuario, string rolNombre)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["JwtSettings:SecretKey"] ?? "ClaveDefault"));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.NombreCompleto),
                new Claim(ClaimTypes.Role, rolNombre)  
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "60")),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}