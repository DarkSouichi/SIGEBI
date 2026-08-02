using System;
using System.Collections.Generic;

namespace SIGEBI.Persistence.Scaffold;

public partial class Prestamo
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public int EjemplarId { get; set; }

    public DateTime FechaPrestamo { get; set; }

    public DateTime FechaDevolucionEsperada { get; set; }

    public DateTime? FechaDevolucionReal { get; set; }

    public int Estado { get; set; }

    public DateTime CreadoEn { get; set; }

    public string CreadoPor { get; set; } = null!;

    public DateTime? ModificadoEn { get; set; }

    public string? ModificadoPor { get; set; }
}
