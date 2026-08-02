using System;
using System.Collections.Generic;

namespace SIGEBI.Persistence.Scaffold;

public partial class Recurso
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string Autor { get; set; } = null!;

    public string Isbn { get; set; } = null!;

    public string Categoria { get; set; } = null!;

    public DateTime CreadoEn { get; set; }

    public string CreadoPor { get; set; } = null!;

    public DateTime? ModificadoEn { get; set; }

    public string? ModificadoPor { get; set; }

    public virtual ICollection<Ejemplare> Ejemplares { get; set; } = new List<Ejemplare>();
}
