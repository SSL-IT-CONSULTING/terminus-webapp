using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus_webapp.Migrations
{
    public partial class dbchange29 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RevenueLineItem_CheckDetails_checkDetailscheckDetailId",
                table: "RevenueLineItem");

            migrationBuilder.DropIndex(
                name: "IX_RevenueLineItem_checkDetailscheckDetailId",
                table: "RevenueLineItem");

            migrationBuilder.DropColumn(
                name: "checkDetailscheckDetailId",
                table: "RevenueLineItem");

            migrationBuilder.AddColumn<string>(
                name: "bankName",
                table: "RevenueLineItem",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "branch",
                table: "RevenueLineItem",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "checkDate",
                table: "RevenueLineItem",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bankName",
                table: "RevenueLineItem");

            migrationBuilder.DropColumn(
                name: "branch",
                table: "RevenueLineItem");

            migrationBuilder.DropColumn(
                name: "checkDate",
                table: "RevenueLineItem");

            migrationBuilder.AddColumn<Guid>(
                name: "checkDetailscheckDetailId",
                table: "RevenueLineItem",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RevenueLineItem_checkDetailscheckDetailId",
                table: "RevenueLineItem",
                column: "checkDetailscheckDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_RevenueLineItem_CheckDetails_checkDetailscheckDetailId",
                table: "RevenueLineItem",
                column: "checkDetailscheckDetailId",
                principalTable: "CheckDetails",
                principalColumn: "checkDetailId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
