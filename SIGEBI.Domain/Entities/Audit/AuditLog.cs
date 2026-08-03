namespace SIGEBI.Domain.Entities.Audit
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string Actor { get; set; } = string.Empty;
        public string Accion { get; set; } = string.Empty;
        public string Modulo { get; set; } = string.Empty;
        public string Resultado { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string? Detalles { get; set; }
    }
}