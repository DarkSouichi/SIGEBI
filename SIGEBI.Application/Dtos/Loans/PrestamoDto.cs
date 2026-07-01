using SIGEBI.Application.Dtos;

namespace SIGEBI.Application.Dtos.Loans
{
    public class PrestamoDto : DtoBase
    {
        public int PrestamoId { get; set; }
        public int UsuarioId { get; set; }
        public int EjemplarId { get; set; } 
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaDevolucionEsperada { get; set; }
        public DateTime? FechaDevolucionReal { get; set; }
        public string Estado { get; set; } = string.Empty;
    }

    public class SavePrestamoDto : PrestamoDto
    {
    }

    public class UpdatePrestamoDto : PrestamoDto
    {
        public int Id { get; set; }
    }

    public class RemovePrestamoDto : DtoBase
    {
        public int Id { get; set; }
        public bool Removed { get; set; }
    }
}