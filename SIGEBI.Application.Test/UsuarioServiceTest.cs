using Moq;
using Xunit;
using Microsoft.Extensions.Configuration;
using SIGEBI.Application.Dtos.Users;
using SIGEBI.Application.Services;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Users;
using SIGEBI.Infrastructure.Audit;
using SIGEBI.Infrastructure.Logger;
using SIGEBI.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.Application.Test
{
    public class UsuarioServiceTest
    {
        private readonly Mock<IUsuarioRepository> _repoMock;
        private readonly Mock<ILoggerService<UsuarioService>> _loggerMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<IAuditLogger> _auditMock;
        private readonly UsuarioService _service;

        public UsuarioServiceTest()
        {
            _repoMock = new Mock<IUsuarioRepository>();
            _loggerMock = new Mock<ILoggerService<UsuarioService>>();
            _configMock = new Mock<IConfiguration>();
            _auditMock = new Mock<IAuditLogger>();

            _service = new UsuarioService(
                _repoMock.Object,
                _loggerMock.Object,
                _configMock.Object,
                _auditMock.Object
            );
        }

        [Fact]
        public async Task GetAll_DebeRetornarExitoso_CuandoHayUsuarios()
        {
            var usuariosFalsos = new List<Usuario>
            {
                new()
                {
                    Id = 1,
                    NombreCompleto = "Juan Perez",
                    Email = "juan@test.com",
                    EstaActivo = true,
                    RolId = 1,
                    CreadoEn = DateTime.UtcNow
                }
            };

            _repoMock.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(usuariosFalsos);

            var result = await _service.GetAll();

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetAll_DebeRetornarFallido_CuandoHayExcepcion()
        {
            _repoMock.Setup(r => r.GetAllAsync())
                     .ThrowsAsync(new Exception("Error de base de datos"));

            var result = await _service.GetAll();

            Assert.False(result.IsSuccess);
            Assert.Equal("Error obteniendo los usuarios", result.Message);
        }

        [Fact]
        public async Task GetById_DebeRetornarFallido_CuandoUsuarioNoExiste()
        {
            _repoMock.Setup(r => r.GetEntityByIdAsync(99))
                     .ReturnsAsync((Usuario?)null);

            var result = await _service.GetById(99);

            Assert.False(result.IsSuccess);
            Assert.Equal("Usuario no encontrado", result.Message);
        }

        [Fact]
        public async Task Save_DebeRetornarExitoso_CuandoDatosValidos()
        {
            var dto = new SaveUsuarioDto
            {
                NombreCompleto = "Maria Lopez",
                Email = "maria@test.com",
                Password = "123456",
                EstaActivo = true,
                RolId = 1,
                ChangeDate = DateTime.UtcNow,
                ChangeUser = 1
            };

            var usuarioCreado = new Usuario
            {
                Id = 5,
                NombreCompleto = dto.NombreCompleto,
                Email = dto.Email,
                EstaActivo = dto.EstaActivo,
                RolId = dto.RolId,
                CreadoEn = dto.ChangeDate
            };

            _repoMock.Setup(r => r.SaveEntityAsync(It.IsAny<Usuario>()))
                     .ReturnsAsync(new OperationResult { IsSuccess = true, Data = usuarioCreado });

            var result = await _service.Save(dto);

            Assert.True(result.IsSuccess);
            _auditMock.Verify(a => a.LogAsync(
                It.IsAny<string>(),
                "CrearUsuario",
                "Usuarios",
                "Exitoso",
                It.IsAny<string>()
            ), Times.Once);
        }

        [Fact]
        public async Task Update_DebeRetornarExitoso_CuandoUsuarioExiste()
        {
            var dto = new UpdateUsuarioDto
            {
                Id = 1,
                NombreCompleto = "Juan Actualizado",
                Email = "juan.actualizado@test.com",
                EstaActivo = true,
                RolId = 2,
                ChangeDate = DateTime.UtcNow,
                ChangeUser = 1
            };

            var usuarioExistente = new Usuario
            {
                Id = 1,
                NombreCompleto = "Juan Perez",
                Email = "juan@test.com",
                EstaActivo = true,
                RolId = 1
            };

            _repoMock.Setup(r => r.GetEntityByIdAsync(1))
                     .ReturnsAsync(usuarioExistente);

            _repoMock.Setup(r => r.UpdateEntityAsync(It.IsAny<Usuario>()))
                     .ReturnsAsync(new OperationResult { IsSuccess = true, Message = "Usuario actualizado correctamente" });

            var result = await _service.Update(dto);

            Assert.True(result.IsSuccess);
            _auditMock.Verify(a => a.LogAsync(
                It.IsAny<string>(),
                "ActualizarUsuario",
                "Usuarios",
                "Exitoso",
                It.IsAny<string>()
            ), Times.Once);
        }

        [Fact]
        public async Task Update_DebeRetornarFallido_CuandoUsuarioNoExiste()
        {
            var dto = new UpdateUsuarioDto
            {
                Id = 999,
                NombreCompleto = "Inexistente",
                Email = "noexiste@test.com",
                EstaActivo = true,
                RolId = 1,
                ChangeDate = DateTime.UtcNow,
                ChangeUser = 1
            };

            _repoMock.Setup(r => r.GetEntityByIdAsync(999))
                     .ReturnsAsync((Usuario?)null);

            var result = await _service.Update(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal("Usuario no encontrado.", result.Message);
            _auditMock.Verify(a => a.LogAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            ), Times.Never);
        }
    }
}