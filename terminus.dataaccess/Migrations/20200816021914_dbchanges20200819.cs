using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus.dataaccess.Migrations
{
    public partial class dbchanges20200819 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "otherResContactNo1",
                table: "Properties",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otherResContactNo2",
                table: "Properties",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otherResContactNo3",
                table: "Properties",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "otherResContactNo1",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "otherResContactNo2",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "otherResContactNo3",
                table: "Properties");
        }
    }
}
