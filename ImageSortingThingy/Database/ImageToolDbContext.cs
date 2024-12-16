using System;
using System.IO;
using ImageSortingThingy.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageSortingThingy.Database;

public class ImageToolDbContext : DbContext
{
    public DbSet<ImageFileListEntryModel> ImageFileModels { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string dbPath = Path.Combine(AppContext.BaseDirectory, "ImageTool.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }
}