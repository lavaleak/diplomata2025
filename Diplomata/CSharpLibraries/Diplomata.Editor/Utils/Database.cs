using System.IO;
using Diplomata.Editor.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Diplomata.Editor.Utils
{
    public class Database : DbContext, IUtil
    {
        public DbSet<Example> Examples { get; set; }

        private string _directoryPath;

        public Database(string directoryPath = null)
        {
            this._directoryPath = directoryPath == null ?
                Path.Combine(Directory.GetCurrentDirectory()) : directoryPath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string diplomataPath = _directoryPath + "/.diplomata";
            Directory.CreateDirectory(diplomataPath);

            string dbPath = diplomataPath + "/diplomata.db";
            options.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildEntity<Example>(modelBuilder, entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("TEXT")
                    .HasMaxLength(256);

                entity.HasIndex(e => e.Name).IsUnique();
            });
        }

        private void BuildEntity<T>(ModelBuilder modelBuilder, System.Action<EntityTypeBuilder<T>> buildAction) where T : DatabaseBaseModel
        {
            modelBuilder.Entity<T>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Guid)
                    .IsRequired()
                    .HasColumnType("TEXT")
                    .HasDefaultValueSql("hex(randomblob(16))");

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnType("DATETIME")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .IsRequired()
                    .HasColumnType("DATETIME")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasIndex(e => e.Guid).IsUnique();

                buildAction.Invoke(entity);
            });
        }
    }
}