using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities.Catalog;
using SIGEBI.Domain.Entities.Loans;
using SIGEBI.Domain.Entities.Notifications;
using SIGEBI.Domain.Entities.Penalties;
using SIGEBI.Domain.Entities.Users;

namespace SIGEBI.Persistence.Context
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Recurso> Recursos { get; set; }
        public DbSet<Ejemplar> Ejemplares { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<Penalizacion> Penalizaciones { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
    }
}
