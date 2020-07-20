using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus.dataaccess.Migrations
{
    public partial class dbchanges43 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "Vendors",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "Vendors",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "address",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "description",
                table: "Vendors");
        }
    }
}
