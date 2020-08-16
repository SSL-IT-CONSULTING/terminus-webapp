using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus.dataaccess.Migrations
{
    public partial class dbchanges20200816b : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuildingCode",
                table: "Properties",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildingCode",
                table: "Properties");
        }
    }
}
