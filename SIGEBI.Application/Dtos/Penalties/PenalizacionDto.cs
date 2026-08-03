using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.Dtos.Penalties
{
    public class PenalizacionDto
    {
        public int PenalizacionId { get; set; }
        public int UsuarioId { get; set; }
        public int PrestamoId { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaEmision { get; set; }
        public DateTime ChangeDate { get; set; }
        public int ChangeUser { get; set; }
    }

    public class SavePenalizacionDto
    {
        [Required(ErrorMessage = "El UsuarioId es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El UsuarioId debe ser mayor a cero.")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El PrestamoId es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El PrestamoId debe ser mayor a cero.")]
        public int PrestamoId { get; set; }

        [Required(ErrorMessage = "El monto es requerido.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero.")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El estado es requerido.")]
        [RegularExpression("^(Activa|Resuelta|Cancelada)$", ErrorMessage = "Estado inválido. Valores permitidos: Activa, Resuelta, Cancelada.")]
        public string Estado { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de emisión es requerida.")]
        public DateTime FechaEmision { get; set; }

        public DateTime ChangeDate { get; set; }
        public int ChangeUser { get; set; }
    }

    public class UpdatePenalizacionDto
    {
        [Required(ErrorMessage = "El Id es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id debe ser mayor a cero.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El UsuarioId es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El UsuarioId debe ser mayor a cero.")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El PrestamoId es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El PrestamoId debe ser mayor a cero.")]
        public int PrestamoId { get; set; }

        [Required(ErrorMessage = "El monto es requerido.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero.")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El estado es requerido.")]
        [RegularExpression("^(Activa|Resuelta|Cancelada)$", ErrorMessage = "Estado inválido. Valores permitidos: Activa, Resuelta, Cancelada.")]
        public string Estado { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de emisión es requerida.")]
        public DateTime FechaEmision { get; set; }

        public DateTime ChangeDate { get; set; }
        public int ChangeUser { get; set; }
    }

}