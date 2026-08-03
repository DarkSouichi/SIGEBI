using System;
using System.Collections.Generic;

namespace SIGEBI.Persistence.Scaffold;

public partial class Notificacione
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public string Tipo { get; set; } = null!;

    public string Mensaje { get; set; } = null!;

    public DateTime EnviadoEn { get; set; }

    public string Canal { get; set; } = null!;

    public DateTime CreadoEn { get; set; }

    public string CreadoPor { get; set; } = null!;

    public DateTime? ModificadoEn { get; set; }

    public string? ModificadoPor { get; set; }
}
