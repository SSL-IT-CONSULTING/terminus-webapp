using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus_webapp.Migrations
{
    public partial class dbchange45 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 36, nullable: false),
                    displayName = table.Column<string>(maxLength: 200, nullable: true),
                    fileName = table.Column<string>(maxLength: 500, nullable: true),
                    documentType = table.Column<string>(maxLength: 20, nullable: true),
                    refKey = table.Column<string>(maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_documentType_refKey",
                table: "Attachments",
                columns: new[] { "documentType", "refKey" },
                unique: true,
                filter: "[documentType] IS NOT NULL AND [refKey] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachments");
        }
    }
}
