using System.ComponentModel.DataAnnotations;
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
        public int TotalEjemplares { get; set; }
        public int EjemplaresDisponibles { get; set; }

        public string? Descripcion { get; set; }
        public DateTime? FechaLanzamiento { get; set; }
    }

    public class SaveRecursoDto
    {
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(200, ErrorMessage = "El título no puede exceder 200 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor es obligatorio.")]
        [StringLength(100, ErrorMessage = "El autor no puede exceder 100 caracteres.")]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ISBN es obligatorio.")]
        [StringLength(20, ErrorMessage = "El ISBN no puede exceder 20 caracteres.")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [StringLength(50, ErrorMessage = "La categoría no puede exceder 50 caracteres.")]
        public string Categoria { get; set; } = string.Empty;

        public string? Descripcion { get; set; }
        public DateTime? FechaLanzamiento { get; set; }

        public DateTime ChangeDate { get; set; }
        public int ChangeUser { get; set; }
    }

    public class UpdateRecursoDto
    {
        [Required(ErrorMessage = "El ID es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID inválido.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(200, ErrorMessage = "El título no puede exceder 200 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor es obligatorio.")]
        [StringLength(100, ErrorMessage = "El autor no puede exceder 100 caracteres.")]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ISBN es obligatorio.")]
        [StringLength(20, ErrorMessage = "El ISBN no puede exceder 20 caracteres.")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [StringLength(50, ErrorMessage = "La categoría no puede exceder 50 caracteres.")]
        public string Categoria { get; set; } = string.Empty;

        public string? Descripcion { get; set; }
        public DateTime? FechaLanzamiento { get; set; }

        public DateTime ChangeDate { get; set; }
        public int ChangeUser { get; set; }
    }
}