using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Epos.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Conpanys",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ConpanyName = table.Column<string>(nullable: true),
                    ConpanyRegNumber = table.Column<string>(nullable: true),
                    ConpanyVatNumber = table.Column<string>(nullable: true),
                    FirstLine = table.Column<string>(nullable: true),
                    County = table.Column<string>(nullable: true),
                    Postcode = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    WebSite = table.Column<string>(nullable: true),
                    Registed = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conpanys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HasCheckoutComplate = table.Column<bool>(nullable: false),
                    Order = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductForCart",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QTY = table.Column<int>(nullable: false),
                    MixAndMatch = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    SnNeed = table.Column<bool>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    OrdersId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductForCart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductForCart_Orders_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductForCart_OrdersId",
                table: "ProductForCart",
                column: "OrdersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Conpanys");

            migrationBuilder.DropTable(
                name: "ProductForCart");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
