using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus.dataaccess.Migrations
{
    public partial class issue117119fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "checkDate",
                table: "CheckDetails",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "depositDate",
                table: "CheckDetails",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "releaseDate",
                table: "CheckDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "depositDate",
                table: "CheckDetails");

            migrationBuilder.DropColumn(
                name: "releaseDate",
                table: "CheckDetails");

            migrationBuilder.AlterColumn<DateTime>(
                name: "checkDate",
                table: "CheckDetails",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
