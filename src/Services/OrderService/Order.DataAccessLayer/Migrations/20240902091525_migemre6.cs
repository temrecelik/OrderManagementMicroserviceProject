using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Order.DataAccessLayer.Migrations
{
    public partial class migemre6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderOutBoxes",
                columns: table => new
                {
                    IdempotenToken = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OccuredON = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderOutBoxes", x => x.IdempotenToken);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderOutBoxes");
        }
    }
}
