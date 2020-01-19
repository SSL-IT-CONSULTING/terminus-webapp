using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus_webapp.Migrations
{
    public partial class dbchange7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Vendor_vendorId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_GLAccounts_companyId",
                table: "GLAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vendor",
                table: "Vendor");

            migrationBuilder.RenameTable(
                name: "Vendor",
                newName: "Vendors");

            migrationBuilder.AddColumn<DateTime>(
                name: "dueDate",
                table: "Expenses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "companyId",
                table: "Vendors",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vendors",
                table: "Vendors",
                column: "vendorId");

            migrationBuilder.CreateIndex(
                name: "IX_GLAccounts_companyId_accountCode",
                table: "GLAccounts",
                columns: new[] { "companyId", "accountCode" },
                unique: true,
                filter: "[companyId] IS NOT NULL AND [accountCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_companyId",
                table: "Vendors",
                column: "companyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Vendors_vendorId",
                table: "Expenses",
                column: "vendorId",
                principalTable: "Vendors",
                principalColumn: "vendorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_Companies_companyId",
                table: "Vendors",
                column: "companyId",
                principalTable: "Companies",
                principalColumn: "companyId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Vendors_vendorId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_Companies_companyId",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_GLAccounts_companyId_accountCode",
                table: "GLAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vendors",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_companyId",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "dueDate",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "companyId",
                table: "Vendors");

            migrationBuilder.RenameTable(
                name: "Vendors",
                newName: "Vendor");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vendor",
                table: "Vendor",
                column: "vendorId");

            migrationBuilder.CreateIndex(
                name: "IX_GLAccounts_companyId",
                table: "GLAccounts",
                column: "companyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Vendor_vendorId",
                table: "Expenses",
                column: "vendorId",
                principalTable: "Vendor",
                principalColumn: "vendorId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
