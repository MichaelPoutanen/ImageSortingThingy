﻿// <auto-generated />
using System;
using ImageSortingThingy.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ImageSortingThingy.Migrations
{
    [DbContext(typeof(ImageToolDbContext))]
    [Migration("20241221164605_AddedConvertedFileModelAndRelationships")]
    partial class AddedConvertedFileModelAndRelationships
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("ImageSortingThingy.Models.ConvertedFileModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ConvertedDateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConvertedFileHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConvertedFilePath")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsConverted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OriginalFileHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OriginalFilePath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ConvertedFileModels");
                });

            modelBuilder.Entity("ImageSortingThingy.Models.ImageFileListEntryModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AbsolutePath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("ConvertedFileId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Crc32Hash")
                        .HasColumnType("TEXT");

                    b.Property<string>("CustomName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("FileCreationDateTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("FileId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ImageCreatedDateTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ConvertedFileId")
                        .IsUnique();

                    b.ToTable("ImageFileModels");
                });

            modelBuilder.Entity("ImageSortingThingy.Models.ImageFileListEntryModel", b =>
                {
                    b.HasOne("ImageSortingThingy.Models.ConvertedFileModel", "ConvertedFileModel")
                        .WithOne("ImageFileModel")
                        .HasForeignKey("ImageSortingThingy.Models.ImageFileListEntryModel", "ConvertedFileId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("ConvertedFileModel");
                });

            modelBuilder.Entity("ImageSortingThingy.Models.ConvertedFileModel", b =>
                {
                    b.Navigation("ImageFileModel");
                });
#pragma warning restore 612, 618
        }
    }
}