using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus.dataaccess.Migrations
{
    public partial class dbchange6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "cashAccountaccountId",
                table: "Expenses",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "journalEntryid",
                table: "Expenses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "receiptNo",
                table: "Expenses",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "taxAmount",
                table: "Expenses",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "vendorId",
                table: "Expenses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "vendorOther",
                table: "Expenses",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Vendor",
                columns: table => new
                {
                    vendorId = table.Column<string>(maxLength: 36, nullable: false),
                    vendorName = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendor", x => x.vendorId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_cashAccountaccountId",
                table: "Expenses",
                column: "cashAccountaccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_journalEntryid",
                table: "Expenses",
                column: "journalEntryid");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_vendorId",
                table: "Expenses",
                column: "vendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_GLAccounts_cashAccountaccountId",
                table: "Expenses",
                column: "cashAccountaccountId",
                principalTable: "GLAccounts",
                principalColumn: "accountId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_JournalEntriesHdr_journalEntryid",
                table: "Expenses",
                column: "journalEntryid",
                principalTable: "JournalEntriesHdr",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Vendor_vendorId",
                table: "Expenses",
                column: "vendorId",
                principalTable: "Vendor",
                principalColumn: "vendorId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_GLAccounts_cashAccountaccountId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_JournalEntriesHdr_journalEntryid",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Vendor_vendorId",
                table: "Expenses");

            migrationBuilder.DropTable(
                name: "Vendor");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_cashAccountaccountId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_journalEntryid",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_vendorId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "cashAccountaccountId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "journalEntryid",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "receiptNo",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "taxAmount",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "vendorId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "vendorOther",
                table: "Expenses");
        }
    }
}
