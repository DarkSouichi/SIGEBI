using System;
using System.Collections.Generic;

namespace SIGEBI.Persistence.Scaffold;

public partial class Usuario
{
    public int Id { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool EstaActivo { get; set; }

    public int RolId { get; set; }

    public int IntentosFallidos { get; set; }

    public DateTime CreadoEn { get; set; }

    public string CreadoPor { get; set; } = null!;

    public DateTime? ModificadoEn { get; set; }

    public string? ModificadoPor { get; set; }
}
