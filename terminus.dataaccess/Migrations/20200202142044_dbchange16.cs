using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus.dataaccess.Migrations
{
    public partial class dbchange16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "inputVatAccountId",
                table: "Vendors",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isVatRegistered",
                table: "Vendors",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_inputVatAccountId",
                table: "Vendors",
                column: "inputVatAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_GLAccounts_inputVatAccountId",
                table: "Vendors",
                column: "inputVatAccountId",
                principalTable: "GLAccounts",
                principalColumn: "accountId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_GLAccounts_inputVatAccountId",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_inputVatAccountId",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "inputVatAccountId",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "isVatRegistered",
                table: "Vendors");
        }
    }
}
