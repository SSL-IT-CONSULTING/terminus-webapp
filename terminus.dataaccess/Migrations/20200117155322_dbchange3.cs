using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus.dataaccess.Migrations
{
    public partial class dbchange3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_GLAccounts_accountId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Revenues_GLAccounts_accountId",
                table: "Revenues");

            migrationBuilder.AlterColumn<Guid>(
                name: "accountId",
                table: "Revenues",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "accountId",
                table: "Expenses",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_GLAccounts_accountId",
                table: "Expenses",
                column: "accountId",
                principalTable: "GLAccounts",
                principalColumn: "accountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Revenues_GLAccounts_accountId",
                table: "Revenues",
                column: "accountId",
                principalTable: "GLAccounts",
                principalColumn: "accountId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_GLAccounts_accountId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Revenues_GLAccounts_accountId",
                table: "Revenues");

            migrationBuilder.AlterColumn<Guid>(
                name: "accountId",
                table: "Revenues",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "accountId",
                table: "Expenses",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_GLAccounts_accountId",
                table: "Expenses",
                column: "accountId",
                principalTable: "GLAccounts",
                principalColumn: "accountId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Revenues_GLAccounts_accountId",
                table: "Revenues",
                column: "accountId",
                principalTable: "GLAccounts",
                principalColumn: "accountId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
