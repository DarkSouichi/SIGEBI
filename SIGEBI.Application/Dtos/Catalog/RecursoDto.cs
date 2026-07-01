using SIGEBI.Application.Dtos;

namespace SIGEBI.Application.Dtos.Catalog
{
    public class RecursoDto : DtoBase
    {
        public int RecursoId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
    }

    public class SaveRecursoDto : RecursoDto
    {
    }

    public class UpdateRecursoDto : RecursoDto
    {
        public int Id { get; set; }
    }

    public class RemoveRecursoDto : DtoBase
    {
        public int Id { get; set; }
        public bool Removed { get; set; }
    }
}