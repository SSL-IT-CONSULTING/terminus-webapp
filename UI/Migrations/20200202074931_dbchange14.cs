using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus_webapp.Migrations
{
    public partial class dbchange14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "remarks",
                table: "JournalEntriesDtl",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReportParameterViewModels",
                columns: table => new
                {
                    Id = table.Column<int>(maxLength: 8, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AsOfDate = table.Column<DateTime>(nullable: false),
                    ReportType = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportParameterViewModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReferenceViewModals",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    ReferenceType = table.Column<string>(maxLength: 100, nullable: true),
                    ReferenceCode = table.Column<string>(maxLength: 200, nullable: true),
                    ReferenceValue = table.Column<string>(maxLength: 2000, nullable: true),
                    AsOfDate = table.Column<DateTime>(nullable: false),
                    ReportParameterViewModelId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferenceViewModals", x => x.id);
                    table.ForeignKey(
                        name: "FK_ReferenceViewModals_ReportParameterViewModels_ReportParameterViewModelId",
                        column: x => x.ReportParameterViewModelId,
                        principalTable: "ReportParameterViewModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReferenceViewModals_ReportParameterViewModelId",
                table: "ReferenceViewModals",
                column: "ReportParameterViewModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReferenceViewModals");

            migrationBuilder.DropTable(
                name: "ReportParameterViewModels");

            migrationBuilder.DropColumn(
                name: "remarks",
                table: "JournalEntriesDtl");
        }
    }
}
