using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus.dataaccess.Migrations
{
    public partial class dbchanges11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyDirectory_Properties_propertyid",
                table: "PropertyDirectory");

            migrationBuilder.DropForeignKey(
               name: "FK_PropertyDirectory_Tenants_tenantid",
             table: "PropertyDirectory");

            migrationBuilder.DropIndex(
               name: "IX_PropertyDirectory_tenantid",
             table: "PropertyDirectory");

            migrationBuilder.DropColumn(
                name: "tenantid",
                table: "PropertyDirectory");

            migrationBuilder.RenameColumn(
                name: "propertyid",
                table: "PropertyDirectory",
                newName: "propertyId");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyDirectory_propertyid",
                table: "PropertyDirectory",
                newName: "IX_PropertyDirectory_propertyId");

            migrationBuilder.AddColumn<decimal>(
                name: "monthlyRate",
                table: "PropertyDirectory",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "revenueAccountId",
                table: "PropertyDirectory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "PropertyDirectory",
                maxLength: 12,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tenandId",
                table: "PropertyDirectory",
                maxLength: 36,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Billings",
                columns: table => new
                {
                    billId = table.Column<Guid>(nullable: false),
                    createdBy = table.Column<string>(maxLength: 100, nullable: true),
                    createDate = table.Column<DateTime>(nullable: false),
                    updatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    updateDate = table.Column<DateTime>(nullable: true),
                    deleted = table.Column<bool>(nullable: false),
                    billRefId = table.Column<string>(maxLength: 500, nullable: true),
                    dateDue = table.Column<DateTime>(nullable: false),
                    amountDue = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    status = table.Column<string>(maxLength: 12, nullable: true),
                    propertyDirectoryId = table.Column<Guid>(nullable: false),
                    companyId = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Billings", x => x.billId);
                    table.ForeignKey(
                        name: "FK_Billings_Companies_companyId",
                        column: x => x.companyId,
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Billings_PropertyDirectory_propertyDirectoryId",
                        column: x => x.propertyDirectoryId,
                        principalTable: "PropertyDirectory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDirectory_revenueAccountId",
                table: "PropertyDirectory",
                column: "revenueAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDirectory_tenandId",
                table: "PropertyDirectory",
                column: "tenandId");

            migrationBuilder.CreateIndex(
                name: "IX_Billings_propertyDirectoryId",
                table: "Billings",
                column: "propertyDirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Billings_companyId_billRefId",
                table: "Billings",
                columns: new[] { "companyId", "billRefId" },
                unique: true,
                filter: "[companyId] IS NOT NULL AND [billRefId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyDirectory_Properties_propertyId",
                table: "PropertyDirectory",
                column: "propertyId",
                principalTable: "Properties",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyDirectory_GLAccounts_revenueAccountId",
                table: "PropertyDirectory",
                column: "revenueAccountId",
                principalTable: "GLAccounts",
                principalColumn: "accountId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyDirectory_Tenants_tenandId",
                table: "PropertyDirectory",
                column: "tenandId",
                principalTable: "Tenants",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyDirectory_Properties_propertyId",
                table: "PropertyDirectory");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyDirectory_GLAccounts_revenueAccountId",
                table: "PropertyDirectory");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyDirectory_Tenants_tenandId",
                table: "PropertyDirectory");

            migrationBuilder.DropTable(
                name: "Billings");

            migrationBuilder.DropIndex(
                name: "IX_PropertyDirectory_revenueAccountId",
                table: "PropertyDirectory");

            migrationBuilder.DropIndex(
                name: "IX_PropertyDirectory_tenandId",
                table: "PropertyDirectory");

            migrationBuilder.DropColumn(
                name: "monthlyRate",
                table: "PropertyDirectory");

            migrationBuilder.DropColumn(
                name: "revenueAccountId",
                table: "PropertyDirectory");

            migrationBuilder.DropColumn(
                name: "status",
                table: "PropertyDirectory");

            migrationBuilder.DropColumn(
                name: "tenandId",
                table: "PropertyDirectory");

            migrationBuilder.RenameColumn(
                name: "propertyId",
                table: "PropertyDirectory",
                newName: "propertyid");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyDirectory_propertyId",
                table: "PropertyDirectory",
                newName: "IX_PropertyDirectory_propertyid");

            migrationBuilder.AddColumn<string>(
                name: "tenantid",
                table: "PropertyDirectory",
                type: "nvarchar(36)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDirectory_tenantid",
                table: "PropertyDirectory",
                column: "tenantid");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyDirectory_Properties_propertyid",
                table: "PropertyDirectory",
                column: "propertyid",
                principalTable: "Properties",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyDirectory_Tenants_tenantid",
                table: "PropertyDirectory",
                column: "tenantid",
                principalTable: "Tenants",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
