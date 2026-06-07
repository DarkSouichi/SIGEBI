using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities.Catalog;

public class Recurso : AuditEntity
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public ICollection<Ejemplar> Ejemplares { get; set; } = [];
}

public class Ejemplar : AuditEntity
{
    public int Id { get; set; }
    public string CodigoBarras { get; set; } = string.Empty;
    public EstadoEjemplar Estado { get; set; } = EstadoEjemplar.Disponible;
    public int RecursoId { get; set; }

    public void CambiarEstado(EstadoEjemplar nuevoEstado) => Estado = nuevoEstado;
}

public enum EstadoEjemplar
{
    Disponible,
    Prestado,
    Reservado,
    FueraDeServicio
}