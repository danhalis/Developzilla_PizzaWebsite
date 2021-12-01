using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaWebsite.Migrations
{
    public partial class Fix_CartItem_ForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_ProductPortions_ProductPortionId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ProductPortionId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "ProductPortionId",
                table: "CartItems");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "ProductPortions",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_PortionId",
                table: "CartItems",
                column: "PortionId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Portions_PortionId",
                table: "CartItems",
                column: "PortionId",
                principalTable: "Portions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Portions_PortionId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_PortionId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "ProductPortions",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

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
    }
}
