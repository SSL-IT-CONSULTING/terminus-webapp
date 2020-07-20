using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus.dataaccess.Migrations
{
    public partial class dbchanges12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntriesDtl_GLAccounts_accountId",
                table: "JournalEntriesDtl");

            migrationBuilder.AddColumn<string>(
                name: "remarks",
                table: "JournalEntriesHdr",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "transactionDate",
                table: "JournalEntriesHdr",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "accountId",
                table: "JournalEntriesDtl",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntriesDtl_GLAccounts_accountId",
                table: "JournalEntriesDtl",
                column: "accountId",
                principalTable: "GLAccounts",
                principalColumn: "accountId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntriesDtl_GLAccounts_accountId",
                table: "JournalEntriesDtl");

            migrationBuilder.DropColumn(
                name: "remarks",
                table: "JournalEntriesHdr");

            migrationBuilder.DropColumn(
                name: "transactionDate",
                table: "JournalEntriesHdr");

            migrationBuilder.AlterColumn<Guid>(
                name: "accountId",
                table: "JournalEntriesDtl",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntriesDtl_GLAccounts_accountId",
                table: "JournalEntriesDtl",
                column: "accountId",
                principalTable: "GLAccounts",
                principalColumn: "accountId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
