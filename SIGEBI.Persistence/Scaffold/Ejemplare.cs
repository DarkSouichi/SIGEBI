using System;
using System.Collections.Generic;

namespace SIGEBI.Persistence.Scaffold;

public partial class Ejemplare
{
    public int Id { get; set; }

    public string CodigoBarras { get; set; } = null!;

    public int Estado { get; set; }

    public int RecursoId { get; set; }

    public DateTime CreadoEn { get; set; }

    public string CreadoPor { get; set; } = null!;

    public DateTime? ModificadoEn { get; set; }

    public string? ModificadoPor { get; set; }

    public virtual Recurso Recurso { get; set; } = null!;
}
