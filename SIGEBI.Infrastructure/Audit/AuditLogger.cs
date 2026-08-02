using Microsoft.Extensions.Logging;
using SIGEBI.Domain.Entities.Audit;
using SIGEBI.Persistence.Context;

namespace SIGEBI.Infrastructure.Audit
{
    public class AuditLogger : IAuditLogger
    {
        private readonly LibraryContext _context;
        private readonly ILogger<AuditLogger> _logger;

        public AuditLogger(LibraryContext context, ILogger<AuditLogger> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task LogAsync(string actor, string accion, string modulo,
                                    string resultado, string? detalles = null)
        {
            try
            {
                _context.AuditLogs.Add(new AuditLog
                {
                    Actor = actor,
                    Accion = accion,
                    Modulo = modulo,
                    Resultado = resultado,
                    Fecha = DateTime.Now,
                    Detalles = detalles
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar en AuditLog: Actor={Actor}, Accion={Accion}, Modulo={Modulo}",
                    actor, accion, modulo);
            }
        }
    }
}