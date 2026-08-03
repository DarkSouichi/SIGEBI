using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities.Audit;
using SIGEBI.Domain.Entities.Catalog;
using SIGEBI.Domain.Entities.Loans;
using SIGEBI.Domain.Entities.Notifications;
using SIGEBI.Domain.Entities.Penalties;
using SIGEBI.Domain.Entities.Users;
using BCrypt;
using BCrypt.Net;

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
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ========================================
            // Datos iniciales para la BD
            // ========================================

            modelBuilder.Entity<Rol>().HasData(
                new Rol
                {
                    Id = 1,
                    Nombre = "Admin",
                    Permisos = "Todos"
                },
                new Rol
                {
                    Id = 2,
                    Nombre = "Usuario",
                    Permisos = "Lectura"
                }
            );

            var adminPasswordHash = BCrypt.Net.BCrypt.HashPassword("123456");

            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    NombreCompleto = "Administrador",
                    Email = "admin@test.com",
                    PasswordHash = adminPasswordHash,
                    EstaActivo = true,
                    RolId = 1,        
                    IntentosFallidos = 0,
                    CreadoEn = DateTime.Now,
                    CreadoPor = "Sistema"
                }
            );
        }
    }
}