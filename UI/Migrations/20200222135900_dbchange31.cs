using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus_webapp.Migrations
{
    public partial class dbchange31 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RevenueMonthlyDueWTAccountId",
                table: "CompanyDefaults",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "checkNo",
                table: "CheckDetails",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDefaults_RevenueMonthlyDueWTAccountId",
                table: "CompanyDefaults",
                column: "RevenueMonthlyDueWTAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyDefaults_GLAccounts_RevenueMonthlyDueWTAccountId",
                table: "CompanyDefaults",
                column: "RevenueMonthlyDueWTAccountId",
                principalTable: "GLAccounts",
                principalColumn: "accountId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyDefaults_GLAccounts_RevenueMonthlyDueWTAccountId",
                table: "CompanyDefaults");

            migrationBuilder.DropIndex(
                name: "IX_CompanyDefaults_RevenueMonthlyDueWTAccountId",
                table: "CompanyDefaults");

            migrationBuilder.DropColumn(
                name: "RevenueMonthlyDueWTAccountId",
                table: "CompanyDefaults");

            migrationBuilder.DropColumn(
                name: "checkNo",
                table: "CheckDetails");
        }
    }
}
