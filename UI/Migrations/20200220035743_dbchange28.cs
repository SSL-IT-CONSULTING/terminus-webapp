using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus_webapp.Migrations
{
    public partial class dbchange28 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyDefaults",
                columns: table => new
                {
                    companyId = table.Column<string>(maxLength: 10, nullable: false),
                    RevenueMonthlyDueDebitAccountId = table.Column<Guid>(nullable: true),
                    RevenueMonthlyDueAccountId = table.Column<Guid>(nullable: true),
                    RevenueMonthlyDueVatAccountId = table.Column<Guid>(nullable: true),
                    RevenueAssocDuesDebitAccountId = table.Column<Guid>(nullable: true),
                    RevenueAssocDuesAccountId = table.Column<Guid>(nullable: true),
                    RevenueAssocDuesVatAccountId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyDefaults", x => x.companyId);
                    table.ForeignKey(
                        name: "FK_CompanyDefaults_GLAccounts_RevenueAssocDuesAccountId",
                        column: x => x.RevenueAssocDuesAccountId,
                        principalTable: "GLAccounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyDefaults_GLAccounts_RevenueAssocDuesDebitAccountId",
                        column: x => x.RevenueAssocDuesDebitAccountId,
                        principalTable: "GLAccounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyDefaults_GLAccounts_RevenueAssocDuesVatAccountId",
                        column: x => x.RevenueAssocDuesVatAccountId,
                        principalTable: "GLAccounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyDefaults_GLAccounts_RevenueMonthlyDueAccountId",
                        column: x => x.RevenueMonthlyDueAccountId,
                        principalTable: "GLAccounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyDefaults_GLAccounts_RevenueMonthlyDueDebitAccountId",
                        column: x => x.RevenueMonthlyDueDebitAccountId,
                        principalTable: "GLAccounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyDefaults_GLAccounts_RevenueMonthlyDueVatAccountId",
                        column: x => x.RevenueMonthlyDueVatAccountId,
                        principalTable: "GLAccounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDefaults_RevenueAssocDuesAccountId",
                table: "CompanyDefaults",
                column: "RevenueAssocDuesAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDefaults_RevenueAssocDuesDebitAccountId",
                table: "CompanyDefaults",
                column: "RevenueAssocDuesDebitAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDefaults_RevenueAssocDuesVatAccountId",
                table: "CompanyDefaults",
                column: "RevenueAssocDuesVatAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDefaults_RevenueMonthlyDueAccountId",
                table: "CompanyDefaults",
                column: "RevenueMonthlyDueAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDefaults_RevenueMonthlyDueDebitAccountId",
                table: "CompanyDefaults",
                column: "RevenueMonthlyDueDebitAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDefaults_RevenueMonthlyDueVatAccountId",
                table: "CompanyDefaults",
                column: "RevenueMonthlyDueVatAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyDefaults");
        }
    }
}
