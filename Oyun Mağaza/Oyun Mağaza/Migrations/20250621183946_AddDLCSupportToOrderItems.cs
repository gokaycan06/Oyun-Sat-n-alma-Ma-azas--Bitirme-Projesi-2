using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oyun_Mağaza.Migrations
{
    /// <inheritdoc />
    public partial class AddDLCSupportToOrderItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DLCId",
                table: "OrderItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_DLCId",
                table: "OrderItems",
                column: "DLCId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_DLCs_DLCId",
                table: "OrderItems",
                column: "DLCId",
                principalTable: "DLCs",
                principalColumn: "DLCId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_DLCs_DLCId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_DLCId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "DLCId",
                table: "OrderItems");
        }
    }
}
