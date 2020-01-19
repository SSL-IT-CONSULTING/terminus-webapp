using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus_webapp.Migrations
{
    public partial class dbchange8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "rowOrder",
                table: "Vendors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rowOrder",
                table: "GLAccounts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_rowOrder",
                table: "Vendors",
                column: "rowOrder");

            migrationBuilder.CreateIndex(
                name: "IX_GLAccounts_rowOrder",
                table: "GLAccounts",
                column: "rowOrder");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vendors_rowOrder",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_GLAccounts_rowOrder",
                table: "GLAccounts");

            migrationBuilder.DropColumn(
                name: "rowOrder",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "rowOrder",
                table: "GLAccounts");
        }
    }
}
