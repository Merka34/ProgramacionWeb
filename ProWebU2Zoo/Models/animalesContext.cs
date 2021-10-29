using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ProWebU2Zoo.Models
{
    public partial class animalesContext : DbContext
    {
        public animalesContext()
        {
        }

        public animalesContext(DbContextOptions<animalesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Clase> Clases { get; set; }
        public virtual DbSet<Especy> Especies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;user=root;password=root;database=animales", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.20-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            modelBuilder.Entity<Clase>(entity =>
            {
                entity.ToTable("clase");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Descripcion).HasColumnType("text");

                entity.Property(e => e.Nombre).HasMaxLength(45);
            });

            modelBuilder.Entity<Especy>(entity =>
            {
                entity.ToTable("especies");

                entity.HasIndex(e => e.IdClase, "fk_especie_clase_idx");

                entity.Property(e => e.Especie)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.Habitat).HasMaxLength(45);

                entity.Property(e => e.Observaciones).HasMaxLength(100);

                entity.Property(e => e.Peso).HasColumnType("double(7,2)");

                entity.HasOne(d => d.IdClaseNavigation)
                    .WithMany(p => p.Especies)
                    .HasForeignKey(d => d.IdClase)
                    .HasConstraintName("fk_especie_clase");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
