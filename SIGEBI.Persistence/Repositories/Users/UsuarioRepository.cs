using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Users;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;

namespace SIGEBI.Persistence.Repositories.Users
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        private readonly LibraryContext _context;

        public UsuarioRepository(LibraryContext context) : base(context)
        {
            _context = context;
        }

        public async Task<OperationResult> GetUsuarioByEmail(string email)
        {
            OperationResult result = new OperationResult();
            try
            {
                var dato = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == email);
                result.Data = dato;
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "Ocurrio un error obteniendo el usuario por email.";
            }
            return result;
        }

        public async Task<OperationResult> GetUsuariosActivos()
        {
            OperationResult result = new OperationResult();
            try
            {
                var datos = await _context.Usuarios
                    .Where(u => u.EstaActivo)
                    .ToListAsync();
                result.Data = datos;
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "Ocurrio un error obteniendo los usuarios activos.";
            }
            return result;
        }

        public async Task<OperationResult> VerificarHabilitacion(int usuarioId)
        {
            OperationResult result = new OperationResult();
            try
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Id == usuarioId);

                if (usuario == null || !usuario.EstaActivo)
                {
                    result.Success = false;
                    result.Message = "El usuario no está activo o no existe.";
                    return result;
                }

                var tienePenalizacion = await _context.Penalizaciones
                    .AnyAsync(p => p.UsuarioId == usuarioId && p.Estado == "Activa");

                if (tienePenalizacion)
                {
                    result.Success = false;
                    result.Message = "El usuario tiene una penalizacion activa.";
                    return result;
                }

                result.Data = true;
                result.Message = "El usuario esta habilitado.";
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "Ocurrio un error verificando la habilitacion del usuario.";
            }
            return result;
        }
    }
}