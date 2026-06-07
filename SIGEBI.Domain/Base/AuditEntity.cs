namespace SIGEBI.Domain.Base;

public abstract class AuditEntity
{
    public DateTime CreadoEn { get; set; } = DateTime.Now;
    public string CreadoPor { get; set; } = string.Empty;
    public DateTime? ModificadoEn { get; set; }
    public string? ModificadoPor { get; set; }
}