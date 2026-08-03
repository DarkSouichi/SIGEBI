using System;
using System.Collections.Generic;

namespace SIGEBI.Persistence.Scaffold;

public partial class Penalizacione
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public int PrestamoId { get; set; }

    public decimal Monto { get; set; }

    public string Estado { get; set; } = null!;

    public DateTime FechaEmision { get; set; }

    public DateTime? FechaResolucion { get; set; }

    public DateTime CreadoEn { get; set; }

    public string CreadoPor { get; set; } = null!;

    public DateTime? ModificadoEn { get; set; }

    public string? ModificadoPor { get; set; }
}
