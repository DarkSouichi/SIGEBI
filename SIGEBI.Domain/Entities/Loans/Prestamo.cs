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
    Pendiente = 0,
    Activo = 1,
    Devuelto = 2,
    Vencido = 3,
    Rechazado = 4 
}