using System.IO;
using Diplomata.Models;
using Godot;
using Microsoft.EntityFrameworkCore;

namespace Diplomata
{
    public class Database : DbContext
    {
        public DbSet<Example> Examples { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string diplomataPath = OS.GetUserDataDir().Replace("\\", "/") + "/.diplomata";
            Directory.CreateDirectory(diplomataPath);
            GD.Print(diplomataPath);

            string dbPath = diplomataPath + "/diplomata.db";
            options.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Example>(entity =>
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

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("TEXT")
                    .HasMaxLength(256);

                entity.HasIndex(e => e.Guid).IsUnique();
                entity.HasIndex(e => e.Name).IsUnique();
            });
        }
    }
}