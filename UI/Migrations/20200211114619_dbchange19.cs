using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus_webapp.Migrations
{
    public partial class dbchange19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "amountDue",
                table: "Billings",
                newName: "totalAmount");

            migrationBuilder.AddColumn<Guid>(
                name: "billId",
                table: "Revenues",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "amountPaid",
                table: "Billings",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "balance",
                table: "Billings",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "propertyId",
                table: "Billings",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tenantId",
                table: "Billings",
                maxLength: 36,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BillingLineItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    description = table.Column<string>(maxLength: 200, nullable: true),
                    amount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    lineNo = table.Column<int>(nullable: false),
                    billingId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingLineItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillingLineItems_Billings_billingId",
                        column: x => x.billingId,
                        principalTable: "Billings",
                        principalColumn: "billId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Revenues_billId",
                table: "Revenues",
                column: "billId");

            migrationBuilder.CreateIndex(
                name: "IX_Billings_propertyId",
                table: "Billings",
                column: "propertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Billings_tenantId",
                table: "Billings",
                column: "tenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BillingLineItems_billingId",
                table: "BillingLineItems",
                column: "billingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Billings_Properties_propertyId",
                table: "Billings",
                column: "propertyId",
                principalTable: "Properties",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Billings_Tenants_tenantId",
                table: "Billings",
                column: "tenantId",
                principalTable: "Tenants",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Revenues_Billings_billId",
                table: "Revenues",
                column: "billId",
                principalTable: "Billings",
                principalColumn: "billId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Billings_Properties_propertyId",
                table: "Billings");

            migrationBuilder.DropForeignKey(
                name: "FK_Billings_Tenants_tenantId",
                table: "Billings");

            migrationBuilder.DropForeignKey(
                name: "FK_Revenues_Billings_billId",
                table: "Revenues");

            migrationBuilder.DropTable(
                name: "BillingLineItems");

            migrationBuilder.DropIndex(
                name: "IX_Revenues_billId",
                table: "Revenues");

            migrationBuilder.DropIndex(
                name: "IX_Billings_propertyId",
                table: "Billings");

            migrationBuilder.DropIndex(
                name: "IX_Billings_tenantId",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "billId",
                table: "Revenues");

            migrationBuilder.DropColumn(
                name: "amountPaid",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "balance",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "propertyId",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "tenantId",
                table: "Billings");

            migrationBuilder.RenameColumn(
                name: "totalAmount",
                table: "Billings",
                newName: "amountDue");
        }
    }
}
