using Microsoft.EntityFrameworkCore.Migrations;

namespace Epos.Migrations
{
    public partial class init4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SN",
                table: "ProductForCart",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SN",
                table: "ProductForCart");
        }
    }
}
