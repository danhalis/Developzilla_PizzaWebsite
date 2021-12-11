using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaWebsite.Migrations
{
    public partial class orderstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeliveryArea",
                table: "UserDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "UserDatas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReceptionMethod",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryArea",
                table: "UserDatas");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "UserDatas");

            migrationBuilder.DropColumn(
                name: "ReceptionMethod",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Orders");
        }
    }
}
