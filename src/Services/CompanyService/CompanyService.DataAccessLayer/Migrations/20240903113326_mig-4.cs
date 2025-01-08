using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyService.DataAccessLayer.Migrations
{
    public partial class mig4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderInboxes",
                columns: table => new
                {
                    IdempotenToken = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Processed = table.Column<bool>(type: "bit", nullable: false),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderInboxes", x => x.IdempotenToken);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderInboxes");
        }
    }
}
