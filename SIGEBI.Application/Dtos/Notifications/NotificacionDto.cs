using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.Dtos.Notifications
{
    public class NotificacionDto
    {
        public int NotificacionId { get; set; }
        public int UsuarioId { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public DateTime EnviadoEn { get; set; }
        public string Canal { get; set; } = string.Empty;
        public int? PrestamoId { get; set; }
        public int? RecursoId { get; set; }

        public DateTime ChangeDate { get; set; }
        public int ChangeUser { get; set; }
    }

 
    public class SaveNotificacionDto
    {
        [Required(ErrorMessage = "El UsuarioId es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El UsuarioId debe ser mayor a cero.")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El tipo es requerido.")]
        [StringLength(50, ErrorMessage = "El tipo no puede tener más de 50 caracteres.")]
        public string Tipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El mensaje es requerido.")]
        public string Mensaje { get; set; } = string.Empty;

        [Required(ErrorMessage = "El canal es requerido.")]
        [StringLength(50, ErrorMessage = "El canal no puede tener más de 50 caracteres.")]
        public string Canal { get; set; } = string.Empty;

        public int? PrestamoId { get; set; }
        public int? RecursoId { get; set; }


        public DateTime ChangeDate { get; set; }
        public int ChangeUser { get; set; }
    }

    public class UpdateNotificacionDto
    {
        [Required(ErrorMessage = "El Id es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El Id debe ser mayor a cero.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El UsuarioId es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El UsuarioId debe ser mayor a cero.")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El tipo es requerido.")]
        [StringLength(50, ErrorMessage = "El tipo no puede tener más de 50 caracteres.")]
        public string Tipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El mensaje es requerido.")]
        public string Mensaje { get; set; } = string.Empty;

        [Required(ErrorMessage = "El canal es requerido.")]
        [StringLength(50, ErrorMessage = "El canal no puede tener más de 50 caracteres.")]
        public string Canal { get; set; } = string.Empty;

        public DateTime EnviadoEn { get; set; }

        public DateTime ChangeDate { get; set; }
        public int ChangeUser { get; set; }
    }


}