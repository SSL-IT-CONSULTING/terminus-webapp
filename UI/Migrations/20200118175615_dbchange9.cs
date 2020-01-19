using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus_webapp.Migrations
{
    public partial class dbchange9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "source",
                table: "JournalEntriesHdr",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "sourceId",
                table: "JournalEntriesHdr",
                maxLength: 36,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "source",
                table: "JournalEntriesHdr");

            migrationBuilder.DropColumn(
                name: "sourceId",
                table: "JournalEntriesHdr");
        }
    }
}
