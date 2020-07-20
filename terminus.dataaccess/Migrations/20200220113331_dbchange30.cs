using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus.dataaccess.Migrations
{
    public partial class dbchange30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentIdTable",
                columns: table => new
                {
                    IdKey = table.Column<string>(maxLength: 200, nullable: false),
                    Format = table.Column<string>(maxLength: 200, nullable: true),
                    NextId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentIdTable", x => x.IdKey);
                    table.ForeignKey(
                        name: "FK_DocumentIdTable_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentIdTable_CompanyId",
                table: "DocumentIdTable",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentIdTable");
        }
    }
}
