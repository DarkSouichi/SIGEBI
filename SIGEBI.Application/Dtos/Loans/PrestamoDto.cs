using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.Dtos.Loans
{
    public class PrestamoDto
    {
        public int PrestamoId { get; set; }
        public int UsuarioId { get; set; }
        public int EjemplarId { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaDevolucionEsperada { get; set; }
        public DateTime? FechaDevolucionReal { get; set; }
        public string Estado { get; set; } = string.Empty;
        public DateTime ChangeDate { get; set; }
        public int ChangeUser { get; set; }
    }

    public class SavePrestamoDto
    {
        [Required(ErrorMessage = "El UsuarioId es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El UsuarioId debe ser mayor a cero.")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El EjemplarId es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El EjemplarId debe ser mayor a cero.")]
        public int EjemplarId { get; set; }

        [Required(ErrorMessage = "La fecha de préstamo es requerida.")]
        public DateTime FechaPrestamo { get; set; }

        [Required(ErrorMessage = "La fecha de devolución esperada es requerida.")]
        public DateTime FechaDevolucionEsperada { get; set; }

        public DateTime? FechaDevolucionReal { get; set; }

        [Required(ErrorMessage = "El estado es requerido.")]
        [RegularExpression("^(Pendiente|Activo|Devuelto|Vencido)$", ErrorMessage = "Estado inválido. Valores permitidos: Pendiente, Activo, Devuelto, Vencido.")]
        public string Estado { get; set; } = string.Empty;

        public DateTime ChangeDate { get; set; }
        public int ChangeUser { get; set; }
    }

    public class UpdatePrestamoDto
    {
        [Required(ErrorMessage = "El Id es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id debe ser mayor a cero.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El UsuarioId es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El UsuarioId debe ser mayor a cero.")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El EjemplarId es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El EjemplarId debe ser mayor a cero.")]
        public int EjemplarId { get; set; }

        [Required(ErrorMessage = "La fecha de préstamo es requerida.")]
        public DateTime FechaPrestamo { get; set; }

        [Required(ErrorMessage = "La fecha de devolución esperada es requerida.")]
        public DateTime FechaDevolucionEsperada { get; set; }

        public DateTime? FechaDevolucionReal { get; set; }

        [Required(ErrorMessage = "El estado es requerido.")]
        [RegularExpression("^(Pendiente|Activo|Devuelto|Vencido)$", ErrorMessage = "Estado inválido. Valores permitidos: Pendiente, Activo, Devuelto, Vencido.")]
        public string Estado { get; set; } = string.Empty;

        public DateTime ChangeDate { get; set; }
        public int ChangeUser { get; set; }
    }

}