using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus_webapp.Migrations
{
    public partial class dbchanges39a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 36, nullable: false),
                    lastName = table.Column<string>(maxLength: 50, nullable: true),
                    lastFirst = table.Column<string>(maxLength: 50, nullable: true),
                    description = table.Column<string>(maxLength: 1000, nullable: true),
                    vatRegistered = table.Column<bool>(nullable: false),
                    address = table.Column<string>(maxLength: 1000, nullable: true),
                    contactNo1 = table.Column<string>(maxLength: 20, nullable: true),
                    contactNo2 = table.Column<string>(maxLength: 20, nullable: true),
                    contactNo3 = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
