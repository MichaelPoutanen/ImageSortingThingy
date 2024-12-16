using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImageSortingThingy.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImageFileModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileName = table.Column<string>(type: "TEXT", nullable: true),
                    AbsolutePath = table.Column<string>(type: "TEXT", nullable: true),
                    FileId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomName = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Crc32Hash = table.Column<string>(type: "TEXT", nullable: true),
                    FileCreationDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ImageCreatedDateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageFileModels", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageFileModels");
        }
    }
}
