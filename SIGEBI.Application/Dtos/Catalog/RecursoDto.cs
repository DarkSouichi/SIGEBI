using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.Dtos.Catalog
{
    public class RecursoDto
    {
        public int RecursoId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public DateTime ChangeDate { get; set; }
        public int ChangeUser { get; set; }
    }

    public class SaveRecursoDto
    {
        [Required(ErrorMessage = "El título es requerido.")]
        [StringLength(200, ErrorMessage = "El título no puede tener más de 200 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor es requerido.")]
        [StringLength(100, ErrorMessage = "El autor no puede tener más de 100 caracteres.")]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ISBN es requerido.")]
        [StringLength(20, ErrorMessage = "El ISBN no puede tener más de 20 caracteres.")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "La categoría es requerida.")]
        [StringLength(50, ErrorMessage = "La categoría no puede tener más de 50 caracteres.")]
        public string Categoria { get; set; } = string.Empty;

        public DateTime ChangeDate { get; set; }
        public int ChangeUser { get; set; }
    }

    public class UpdateRecursoDto
    {
        [Required(ErrorMessage = "El Id es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id debe ser mayor a cero.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es requerido.")]
        [StringLength(200, ErrorMessage = "El título no puede tener más de 200 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor es requerido.")]
        [StringLength(100, ErrorMessage = "El autor no puede tener más de 100 caracteres.")]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ISBN es requerido.")]
        [StringLength(20, ErrorMessage = "El ISBN no puede tener más de 20 caracteres.")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "La categoría es requerida.")]
        [StringLength(50, ErrorMessage = "La categoría no puede tener más de 50 caracteres.")]
        public string Categoria { get; set; } = string.Empty;

        public DateTime ChangeDate { get; set; }
        public int ChangeUser { get; set; }
    }

}