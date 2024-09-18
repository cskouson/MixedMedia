using Microsoft.EntityFrameworkCore;
using MixedMedia.Data.Entities;

namespace MixedMedia.Data
{
    public class MixedDbContext:DbContext
    {
        public MixedDbContext(DbContextOptions<MixedDbContext> options) : base(options) { }

        public DbSet<ImageEntity> Images { get; set; }
        public DbSet<VideoEntity> Videos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("en_US.UTF-8")
                .HasPostgresExtension("extensions", "pg_repack")
                .HasPostgresExtension("extensions", "pg_stat_statements");

            modelBuilder.Entity<ImageEntity>(entity =>
            {
                entity.ToTable("image", "process");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.Date)
                    .HasColumnName("date");
                entity.Property(e => e.Description)
                    .HasColumnName("description");
                entity.Property(e => e.Name)
                    .HasColumnName("name");
                entity.Property(e => e.Path)
                    .HasColumnName("path");
            });

            modelBuilder.Entity<VideoEntity>(entity =>
            {
                entity.ToTable("video", "process");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.Date)
                    .HasColumnName("date");
                entity.Property(e => e.Description)
                    .HasColumnName("description");
                entity.Property(e => e.Name)
                    .HasColumnName("name");
                entity.Property(e => e.Path)
                    .HasColumnName("path");
            });
        }
    }
}
