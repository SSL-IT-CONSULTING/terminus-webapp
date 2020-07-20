using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus.dataaccess.Migrations
{
    public partial class dbchanges41 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ownerAddress",
                table: "Properties",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ownerContactNo",
                table: "Properties",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ownerEmailAdd",
                table: "Properties",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ownerFirstName",
                table: "Properties",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ownerLastName",
                table: "Properties",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ownerMiddleName",
                table: "Properties",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ownerRemarks",
                table: "Properties",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ownerAddress",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "ownerContactNo",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "ownerEmailAdd",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "ownerFirstName",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "ownerLastName",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "ownerMiddleName",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "ownerRemarks",
                table: "Properties");
        }
    }
}
