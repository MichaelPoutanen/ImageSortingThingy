using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImageSortingThingy.Migrations
{
    /// <inheritdoc />
    public partial class AddedConvertedFileModelAndRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "ImageFileModels",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AbsolutePath",
                table: "ImageFileModels",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConvertedFileId",
                table: "ImageFileModels",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ConvertedFileModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OriginalFilePath = table.Column<string>(type: "TEXT", nullable: false),
                    OriginalFileHash = table.Column<string>(type: "TEXT", nullable: false),
                    IsConverted = table.Column<bool>(type: "INTEGER", nullable: false),
                    ConvertedFilePath = table.Column<string>(type: "TEXT", nullable: true),
                    ConvertedFileHash = table.Column<string>(type: "TEXT", nullable: true),
                    ConvertedDateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConvertedFileModels", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageFileModels_ConvertedFileId",
                table: "ImageFileModels",
                column: "ConvertedFileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ImageFileModels_ConvertedFileModels_ConvertedFileId",
                table: "ImageFileModels",
                column: "ConvertedFileId",
                principalTable: "ConvertedFileModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageFileModels_ConvertedFileModels_ConvertedFileId",
                table: "ImageFileModels");

            migrationBuilder.DropTable(
                name: "ConvertedFileModels");

            migrationBuilder.DropIndex(
                name: "IX_ImageFileModels_ConvertedFileId",
                table: "ImageFileModels");

            migrationBuilder.DropColumn(
                name: "ConvertedFileId",
                table: "ImageFileModels");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "ImageFileModels",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "AbsolutePath",
                table: "ImageFileModels",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
