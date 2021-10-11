using System;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Minions.Data
{
    public partial class MinionsContext : DbContext
    {
        public MinionsContext()
        {
        }

        public MinionsContext(DbContextOptions<MinionsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<EvilnessFactor> EvilnessFactors { get; set; }
        public virtual DbSet<Minion> Minions { get; set; }
        public virtual DbSet<MinionsVillain> MinionsVillains { get; set; }
        public virtual DbSet<Town> Towns { get; set; }
        public virtual DbSet<Villain> Villains { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;Database=Minions;Trusted_Connection=True;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<EvilnessFactor>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Minion>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Town)
                    .WithMany(p => p.Minions)
                    .HasForeignKey(d => d.TownId)
                    .HasConstraintName("FK_Minions_Towns");
            });

            modelBuilder.Entity<MinionsVillain>(entity =>
            {
                entity.HasKey(e => new { e.MinionId, e.VillainId });

                entity.HasOne(d => d.Minion)
                    .WithMany(p => p.MinionsVillains)
                    .HasForeignKey(d => d.MinionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MinionsVillains_Minions1");

                entity.HasOne(d => d.Villain)
                    .WithMany(p => p.MinionsVillains)
                    .HasForeignKey(d => d.VillainId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MinionsVillains_Villains1");
            });

            modelBuilder.Entity<Town>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.CountryCodeNavigation)
                    .WithMany(p => p.Towns)
                    .HasForeignKey(d => d.CountryCode)
                    .HasConstraintName("FK_Towns_Countries");
            });

            modelBuilder.Entity<Villain>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.EvilnessFactor)
                    .WithMany(p => p.Villains)
                    .HasForeignKey(d => d.EvilnessFactorId)
                    .HasConstraintName("FK_Villains_EvilnessFactors");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
