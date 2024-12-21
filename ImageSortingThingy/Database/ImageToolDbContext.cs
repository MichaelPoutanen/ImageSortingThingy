using System;
using System.IO;
using ImageSortingThingy.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ImageSortingThingy.Database;

public class ImageToolDbContext : DbContext
{
    public DbSet<ImageFileListEntryModel> ImageFileModels { get; set; }
    public DbSet<ConvertedFileModel> ConvertedFileModels { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string dbPath = Path.Combine(AppContext.BaseDirectory, "ImageTool.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.NonTransactionalMigrationOperationWarning));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ImageFileListEntryModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FileName).IsRequired();
            entity.Property(e => e.AbsolutePath).IsRequired();

            entity.HasOne(e => e.ConvertedFileModel)
                .WithOne(c => c.ImageFileModel)
                .HasForeignKey<ImageFileListEntryModel>(e => e.ConvertedFileId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<ConvertedFileModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OriginalFileHash).IsRequired();
            entity.Property(e => e.OriginalFilePath).IsRequired();
            entity.Property(e => e.IsConverted).IsRequired();
        });
    }
}