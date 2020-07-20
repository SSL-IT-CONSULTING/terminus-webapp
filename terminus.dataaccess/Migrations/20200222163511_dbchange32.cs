using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus.dataaccess.Migrations
{
    public partial class dbchange32 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentIdTable_Companies_CompanyId",
                table: "DocumentIdTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentIdTable",
                table: "DocumentIdTable");

            migrationBuilder.AlterColumn<string>(
                name: "documentId",
                table: "Revenues",
                maxLength: 36,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                table: "DocumentIdTable",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentIdTable",
                table: "DocumentIdTable",
                columns: new[] { "IdKey", "CompanyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentIdTable_Companies_CompanyId",
                table: "DocumentIdTable",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "companyId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentIdTable_Companies_CompanyId",
                table: "DocumentIdTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentIdTable",
                table: "DocumentIdTable");

            migrationBuilder.AlterColumn<string>(
                name: "documentId",
                table: "Revenues",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 36,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                table: "DocumentIdTable",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 10);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentIdTable",
                table: "DocumentIdTable",
                column: "IdKey");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentIdTable_Companies_CompanyId",
                table: "DocumentIdTable",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "companyId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
