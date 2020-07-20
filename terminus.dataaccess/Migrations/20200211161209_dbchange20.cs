using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus.dataaccess.Migrations
{
    public partial class dbchange20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Billings_Properties_propertyId",
                table: "Billings");

            migrationBuilder.DropForeignKey(
                name: "FK_Billings_Tenants_tenantId",
                table: "Billings");

            migrationBuilder.DropIndex(
                name: "IX_Billings_propertyId",
                table: "Billings");

            migrationBuilder.DropIndex(
                name: "IX_Billings_tenantId",
                table: "Billings");

            migrationBuilder.DropIndex(
                name: "IX_Billings_companyId_billRefId",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "billRefId",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "propertyId",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "tenantId",
                table: "Billings");

            migrationBuilder.AddColumn<decimal>(
                name: "associationDues",
                table: "PropertyDirectory",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "penaltyPct",
                table: "PropertyDirectory",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ratePerSQM",
                table: "PropertyDirectory",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "totalBalance",
                table: "PropertyDirectory",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "areaInSqm",
                table: "Properties",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "propertyType",
                table: "Properties",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "documentId",
                table: "Billings",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "transactionDate",
                table: "Billings",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "generated",
                table: "BillingLineItems",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Billings_companyId_documentId",
                table: "Billings",
                columns: new[] { "companyId", "documentId" },
                unique: true,
                filter: "[companyId] IS NOT NULL AND [documentId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Billings_companyId_documentId",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "associationDues",
                table: "PropertyDirectory");

            migrationBuilder.DropColumn(
                name: "penaltyPct",
                table: "PropertyDirectory");

            migrationBuilder.DropColumn(
                name: "ratePerSQM",
                table: "PropertyDirectory");

            migrationBuilder.DropColumn(
                name: "totalBalance",
                table: "PropertyDirectory");

            migrationBuilder.DropColumn(
                name: "areaInSqm",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "propertyType",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "documentId",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "transactionDate",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "generated",
                table: "BillingLineItems");

            migrationBuilder.AddColumn<string>(
                name: "billRefId",
                table: "Billings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "propertyId",
                table: "Billings",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tenantId",
                table: "Billings",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Billings_propertyId",
                table: "Billings",
                column: "propertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Billings_tenantId",
                table: "Billings",
                column: "tenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Billings_companyId_billRefId",
                table: "Billings",
                columns: new[] { "companyId", "billRefId" },
                unique: true,
                filter: "[companyId] IS NOT NULL AND [billRefId] IS NOT NULL");

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
        }
    }
}
