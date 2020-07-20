using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus.dataaccess.Migrations
{
    public partial class dbchange18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "documentId",
                table: "Revenues",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "JournalEntriesHdr",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(120)",
                oldMaxLength: 120,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "documentId",
                table: "JournalEntriesHdr",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "documentId",
                table: "Expenses",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "documentId",
                table: "Revenues");

            migrationBuilder.DropColumn(
                name: "documentId",
                table: "JournalEntriesHdr");

            migrationBuilder.DropColumn(
                name: "documentId",
                table: "Expenses");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "JournalEntriesHdr",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
