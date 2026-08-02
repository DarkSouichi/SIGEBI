using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Dtos.Catalog;
using SIGEBI.Application.Interfaces;
using SIGEBI.Infrastructure.Logger;
using System.Security.Claims;

namespace SIGEBI.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EjemplarController : ControllerBase
    {
        private readonly IEjemplarService _ejemplarService;
        private readonly ILoggerService<EjemplarController> _logger;

        public EjemplarController(IEjemplarService ejemplarService,
                                  ILoggerService<EjemplarController> logger)
        {
            _ejemplarService = ejemplarService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _ejemplarService.GetAll();
            if (result.IsSuccess)
                return Ok(result);
            else
            {
                _logger.LogError($"Error obteniendo ejemplares: {result.Message}", new Exception(result.Message));
                return BadRequest(result);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _ejemplarService.GetById(id);
            if (result.IsSuccess)
                return Ok(result);
            else
            {
                _logger.LogError($"Error obteniendo ejemplar: {result.Message}", new Exception(result.Message));
                return BadRequest(result);
            }
        }

        [HttpPost("CrearEjemplar")]
        public async Task<IActionResult> Create([FromBody] SaveEjemplarDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "1");
            dto.ChangeDate = DateTime.Now;
            dto.ChangeUser = userId;

            var result = await _ejemplarService.Save(dto);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("ActualizarEjemplar")]
        public async Task<IActionResult> Update([FromBody] UpdateEjemplarDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "1");
            dto.ChangeDate = DateTime.Now;
            dto.ChangeUser = userId;

            var result = await _ejemplarService.Update(dto);
            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}