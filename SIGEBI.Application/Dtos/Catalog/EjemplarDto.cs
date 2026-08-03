using SIGEBI.Application.Dtos;

namespace SIGEBI.Application.Dtos.Catalog
{
    public class EjemplarDto : DtoBase
    {
        public int EjemplarId { get; set; }
        public string CodigoBarras { get; set; } = string.Empty;
        public int Estado { get; set; } 
        public int RecursoId { get; set; }
    }

    public class SaveEjemplarDto
    {
        public string CodigoBarras { get; set; } = string.Empty;
        public int Estado { get; set; }
        public int RecursoId { get; set; }
        public DateTime ChangeDate { get; set; }
        public int ChangeUser { get; set; }
    }

    public class UpdateEjemplarDto
    {
        public int Id { get; set; }
        public string CodigoBarras { get; set; } = string.Empty;
        public int Estado { get; set; }
        public int RecursoId { get; set; }
        public DateTime ChangeDate { get; set; }
        public int ChangeUser { get; set; }
    }

    public class DesactivarEjemplarDto
    {
        public int Id { get; set; }
        public int ChangeUser { get; set; }
    }
}