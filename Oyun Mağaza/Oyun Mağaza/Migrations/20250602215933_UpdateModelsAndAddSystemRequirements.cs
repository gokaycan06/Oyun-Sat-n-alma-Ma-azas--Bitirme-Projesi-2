using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oyun_Mağaza.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelsAndAddSystemRequirements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DirectX",
                table: "SystemRequirements");

            migrationBuilder.DropColumn(
                name: "IsMinimum",
                table: "SystemRequirements");

            migrationBuilder.DropColumn(
                name: "FoundedDate",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "FoundedDate",
                table: "Developers");

            migrationBuilder.RenameColumn(
                name: "RequirementId",
                table: "SystemRequirements",
                newName: "SystemRequirementId");

            migrationBuilder.RenameColumn(
                name: "Logo",
                table: "Publishers",
                newName: "LogoUrl");

            migrationBuilder.RenameColumn(
                name: "HasSubtitles",
                table: "GameLanguages",
                newName: "Subtitles");

            migrationBuilder.RenameColumn(
                name: "HasInterface",
                table: "GameLanguages",
                newName: "Interface");

            migrationBuilder.RenameColumn(
                name: "HasAudio",
                table: "GameLanguages",
                newName: "Audio");

            migrationBuilder.RenameColumn(
                name: "Logo",
                table: "Developers",
                newName: "LogoUrl");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "SystemRequirements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Publishers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FoundedYear",
                table: "Publishers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Developers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FoundedYear",
                table: "Developers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Video",
                columns: table => new
                {
                    VideoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Video", x => x.VideoId);
                    table.ForeignKey(
                        name: "FK_Video_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Video_GameId",
                table: "Video",
                column: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Video");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "SystemRequirements");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "FoundedYear",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Developers");

            migrationBuilder.DropColumn(
                name: "FoundedYear",
                table: "Developers");

            migrationBuilder.RenameColumn(
                name: "SystemRequirementId",
                table: "SystemRequirements",
                newName: "RequirementId");

            migrationBuilder.RenameColumn(
                name: "LogoUrl",
                table: "Publishers",
                newName: "Logo");

            migrationBuilder.RenameColumn(
                name: "Subtitles",
                table: "GameLanguages",
                newName: "HasSubtitles");

            migrationBuilder.RenameColumn(
                name: "Interface",
                table: "GameLanguages",
                newName: "HasInterface");

            migrationBuilder.RenameColumn(
                name: "Audio",
                table: "GameLanguages",
                newName: "HasAudio");

            migrationBuilder.RenameColumn(
                name: "LogoUrl",
                table: "Developers",
                newName: "Logo");

            migrationBuilder.AddColumn<string>(
                name: "DirectX",
                table: "SystemRequirements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMinimum",
                table: "SystemRequirements",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FoundedDate",
                table: "Publishers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FoundedDate",
                table: "Developers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
