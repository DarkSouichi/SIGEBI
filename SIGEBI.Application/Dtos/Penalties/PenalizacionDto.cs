using SIGEBI.Application.Dtos;

namespace SIGEBI.Application.Dtos.Penalties
{
    public class PenalizacionDto : DtoBase
    {
        public int PenalizacionId { get; set; }
        public int UsuarioId { get; set; }
        public int PrestamoId { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaEmision { get; set; }
    }

    public class SavePenalizacionDto : PenalizacionDto
    {
    }

    public class UpdatePenalizacionDto : PenalizacionDto
    {
        public int Id { get; set; }
    }

    public class RemovePenalizacionDto : DtoBase
    {
        public int Id { get; set; }
        public bool Removed { get; set; }
    }
}