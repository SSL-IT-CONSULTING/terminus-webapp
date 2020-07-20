using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus.dataaccess.Migrations
{
    public partial class dbchange27 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Revenues_GLAccounts_accountId",
                table: "Revenues");

            migrationBuilder.DropForeignKey(
                name: "FK_Revenues_GLAccounts_cashAccountaccountId",
                table: "Revenues");

            migrationBuilder.DropIndex(
                name: "IX_Revenues_accountId",
                table: "Revenues");

            migrationBuilder.DropIndex(
                name: "IX_Revenues_cashAccountaccountId",
                table: "Revenues");

            migrationBuilder.DropColumn(
                name: "accountId",
                table: "Revenues");

            migrationBuilder.DropColumn(
                name: "cashAccountaccountId",
                table: "Revenues");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "accountId",
                table: "Revenues",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "cashAccountaccountId",
                table: "Revenues",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Revenues_accountId",
                table: "Revenues",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_Revenues_cashAccountaccountId",
                table: "Revenues",
                column: "cashAccountaccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Revenues_GLAccounts_accountId",
                table: "Revenues",
                column: "accountId",
                principalTable: "GLAccounts",
                principalColumn: "accountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Revenues_GLAccounts_cashAccountaccountId",
                table: "Revenues",
                column: "cashAccountaccountId",
                principalTable: "GLAccounts",
                principalColumn: "accountId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
