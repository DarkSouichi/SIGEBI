using System;
using System.Collections.Generic;

namespace SIGEBI.Persistence.Scaffold;

public partial class AuditLog
{
    public int Id { get; set; }

    public string Actor { get; set; } = null!;

    public string Accion { get; set; } = null!;

    public string Modulo { get; set; } = null!;

    public string Resultado { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public string? Detalles { get; set; }
}
