using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus.dataaccess.Migrations
{
    public partial class dbchanges35 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AsOfDate",
                table: "ReportParameterViewModels");

            migrationBuilder.AddColumn<DateTime>(
                name: "dateFrom",
                table: "ReportParameterViewModels",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "dateTo",
                table: "ReportParameterViewModels",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "propertyType",
                table: "Properties",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TenantDocuments",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    createdBy = table.Column<string>(maxLength: 100, nullable: true),
                    createDate = table.Column<DateTime>(nullable: false),
                    updatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    updateDate = table.Column<DateTime>(nullable: true),
                    deleted = table.Column<bool>(nullable: false),
                    propertyDirectoryId = table.Column<Guid>(nullable: false),
                    fileName = table.Column<string>(maxLength: 500, nullable: true),
                    filePath = table.Column<string>(maxLength: 1000, nullable: true),
                    extName = table.Column<string>(maxLength: 20, nullable: true),
                    fileDesc = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantDocuments", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TenantDocuments");

            migrationBuilder.DropColumn(
                name: "dateFrom",
                table: "ReportParameterViewModels");

            migrationBuilder.DropColumn(
                name: "dateTo",
                table: "ReportParameterViewModels");

            migrationBuilder.AddColumn<DateTime>(
                name: "AsOfDate",
                table: "ReportParameterViewModels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "propertyType",
                table: "Properties",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);
        }
    }
}
