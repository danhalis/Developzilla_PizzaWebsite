using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaWebsite.Migrations
{
    public partial class delivererorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DevilvererId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DevilvererId",
                table: "Orders");
        }
    }
}
