using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaWebsite.Migrations
{
    public partial class favorite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserDataId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FavoriteItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserDataId",
                table: "Orders",
                column: "UserDataId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteItems_ProductId",
                table: "FavoriteItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_UserDatas_UserDataId",
                table: "Orders",
                column: "UserDataId",
                principalTable: "UserDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_UserDatas_UserDataId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "FavoriteItems");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserDataId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserDataId",
                table: "Orders");
        }
    }
}
