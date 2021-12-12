using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaWebsite.Migrations
{
    public partial class Update_CartItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems");

            migrationBuilder.AddColumn<int>(
                name: "PortionId",
                table: "CartItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductPortionId",
                table: "CartItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductPortionId",
                table: "CartItems",
                column: "ProductPortionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_ProductPortions_ProductPortionId",
                table: "CartItems",
                column: "ProductPortionId",
                principalTable: "ProductPortions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_ProductPortions_ProductPortionId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ProductPortionId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "PortionId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "ProductPortionId",
                table: "CartItems");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
