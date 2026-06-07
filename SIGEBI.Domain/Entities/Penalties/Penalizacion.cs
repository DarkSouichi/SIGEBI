using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities.Penalties;

public class Penalizacion : AuditEntity
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int PrestamoId { get; set; }
    public decimal Monto { get; set; }
    public string Estado { get; set; } = "Activa";
    public DateTime FechaEmision { get; set; }
    public DateTime? FechaResolucion { get; set; }
}