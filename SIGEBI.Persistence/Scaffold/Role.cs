using System;
using System.Collections.Generic;

namespace SIGEBI.Persistence.Scaffold;

public partial class Role
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Permisos { get; set; } = null!;
}
