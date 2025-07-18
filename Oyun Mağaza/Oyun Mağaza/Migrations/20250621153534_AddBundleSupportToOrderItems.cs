using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oyun_Mağaza.Migrations
{
    /// <inheritdoc />
    public partial class AddBundleSupportToOrderItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Games_GameId",
                table: "OrderItems");

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "OrderItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "BundleId",
                table: "OrderItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_BundleId",
                table: "OrderItems",
                column: "BundleId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Bundles_BundleId",
                table: "OrderItems",
                column: "BundleId",
                principalTable: "Bundles",
                principalColumn: "BundleId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Games_GameId",
                table: "OrderItems",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Bundles_BundleId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Games_GameId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_BundleId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "BundleId",
                table: "OrderItems");

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Games_GameId",
                table: "OrderItems",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
