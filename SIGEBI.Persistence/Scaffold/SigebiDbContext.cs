using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SIGEBI.Persistence.Scaffold;

public partial class SigebiDbContext : DbContext
{
    public SigebiDbContext(DbContextOptions<SigebiDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Ejemplare> Ejemplares { get; set; }

    public virtual DbSet<Notificacione> Notificaciones { get; set; }

    public virtual DbSet<Penalizacione> Penalizaciones { get; set; }

    public virtual DbSet<Prestamo> Prestamos { get; set; }

    public virtual DbSet<Recurso> Recursos { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ejemplare>(entity =>
        {
            entity.HasIndex(e => e.RecursoId, "IX_Ejemplares_RecursoId");

            entity.HasOne(d => d.Recurso).WithMany(p => p.Ejemplares).HasForeignKey(d => d.RecursoId);
        });

        modelBuilder.Entity<Penalizacione>(entity =>
        {
            entity.Property(e => e.Monto).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Recurso>(entity =>
        {
            entity.Property(e => e.Isbn).HasColumnName("ISBN");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
