using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus.dataaccess.Migrations
{
    public partial class dbchange21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MonthYear",
                table: "Billings",
                maxLength: 8,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "billType",
                table: "Billings",
                maxLength: 10,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RevenueLineItem",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    billingLineItemId = table.Column<Guid>(nullable: false),
                    debitAccountId = table.Column<Guid>(nullable: false),
                    creditAccountId = table.Column<Guid>(nullable: false),
                    description = table.Column<string>(maxLength: 200, nullable: true),
                    amount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    cashOrCheck = table.Column<string>(maxLength: 1, nullable: true),
                    checkDetailscheckDetailId = table.Column<Guid>(nullable: true),
                    Revenueid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevenueLineItem", x => x.id);
                    table.ForeignKey(
                        name: "FK_RevenueLineItem_Revenues_Revenueid",
                        column: x => x.Revenueid,
                        principalTable: "Revenues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RevenueLineItem_BillingLineItems_billingLineItemId",
                        column: x => x.billingLineItemId,
                        principalTable: "BillingLineItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RevenueLineItem_CheckDetails_checkDetailscheckDetailId",
                        column: x => x.checkDetailscheckDetailId,
                        principalTable: "CheckDetails",
                        principalColumn: "checkDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RevenueLineItem_GLAccounts_creditAccountId",
                        column: x => x.creditAccountId,
                        principalTable: "GLAccounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RevenueLineItem_GLAccounts_debitAccountId",
                        column: x => x.debitAccountId,
                        principalTable: "GLAccounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RevenueLineItem_Revenueid",
                table: "RevenueLineItem",
                column: "Revenueid");

            migrationBuilder.CreateIndex(
                name: "IX_RevenueLineItem_billingLineItemId",
                table: "RevenueLineItem",
                column: "billingLineItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RevenueLineItem_checkDetailscheckDetailId",
                table: "RevenueLineItem",
                column: "checkDetailscheckDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_RevenueLineItem_creditAccountId",
                table: "RevenueLineItem",
                column: "creditAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_RevenueLineItem_debitAccountId",
                table: "RevenueLineItem",
                column: "debitAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RevenueLineItem");

            migrationBuilder.DropColumn(
                name: "MonthYear",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "billType",
                table: "Billings");
        }
    }
}
