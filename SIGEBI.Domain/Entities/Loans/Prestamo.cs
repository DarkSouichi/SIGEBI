using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities.Loans;

public class Prestamo : AuditEntity
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int EjemplarId { get; set; }
    public DateTime FechaPrestamo { get; set; }
    public DateTime FechaDevolucionEsperada { get; set; }
    public DateTime? FechaDevolucionReal { get; set; }
    public EstadoPrestamo Estado { get; set; } = EstadoPrestamo.Pendiente;
}

public enum EstadoPrestamo
{
    Pendiente,
    Activo,
    Devuelto,
    Vencido
}