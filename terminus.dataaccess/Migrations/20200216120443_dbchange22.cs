using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus.dataaccess.Migrations
{
    public partial class dbchange22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RevenueLineItem_Revenues_Revenueid",
                table: "RevenueLineItem");

            migrationBuilder.RenameColumn(
                name: "Revenueid",
                table: "RevenueLineItem",
                newName: "revenueId");

            migrationBuilder.RenameIndex(
                name: "IX_RevenueLineItem_Revenueid",
                table: "RevenueLineItem",
                newName: "IX_RevenueLineItem_revenueId");

            migrationBuilder.AlterColumn<Guid>(
                name: "revenueId",
                table: "RevenueLineItem",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RevenueLineItem_Revenues_revenueId",
                table: "RevenueLineItem",
                column: "revenueId",
                principalTable: "Revenues",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RevenueLineItem_Revenues_revenueId",
                table: "RevenueLineItem");

            migrationBuilder.RenameColumn(
                name: "revenueId",
                table: "RevenueLineItem",
                newName: "Revenueid");

            migrationBuilder.RenameIndex(
                name: "IX_RevenueLineItem_revenueId",
                table: "RevenueLineItem",
                newName: "IX_RevenueLineItem_Revenueid");

            migrationBuilder.AlterColumn<Guid>(
                name: "Revenueid",
                table: "RevenueLineItem",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_RevenueLineItem_Revenues_Revenueid",
                table: "RevenueLineItem",
                column: "Revenueid",
                principalTable: "Revenues",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
