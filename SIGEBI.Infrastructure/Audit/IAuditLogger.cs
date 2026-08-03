namespace SIGEBI.Infrastructure.Audit
{
    public interface IAuditLogger
    {
        Task LogAsync(string actor, string accion, string modulo,
                      string resultado, string? detalles = null);
    }
}