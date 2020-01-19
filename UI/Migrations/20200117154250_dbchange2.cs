using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus_webapp.Migrations
{
    public partial class dbchange2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Revenues_Properties_propertyid",
                table: "Revenues");

            migrationBuilder.DropForeignKey(
                name: "FK_Revenues_Tenants_tenantid",
                table: "Revenues");

            migrationBuilder.DropIndex(
                name: "IX_Revenues_propertyid",
                table: "Revenues");

            migrationBuilder.DropIndex(
                name: "IX_Revenues_tenantid",
                table: "Revenues");

            migrationBuilder.DropColumn(
                name: "propertyid",
                table: "Revenues");

            migrationBuilder.DropColumn(
                name: "tenantid",
                table: "Revenues");

            migrationBuilder.AddColumn<Guid>(
                name: "propertyDirectoryid",
                table: "Revenues",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Revenues_propertyDirectoryid",
                table: "Revenues",
                column: "propertyDirectoryid");

            migrationBuilder.AddForeignKey(
                name: "FK_Revenues_PropertyDirectory_propertyDirectoryid",
                table: "Revenues",
                column: "propertyDirectoryid",
                principalTable: "PropertyDirectory",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Revenues_PropertyDirectory_propertyDirectoryid",
                table: "Revenues");

            migrationBuilder.DropIndex(
                name: "IX_Revenues_propertyDirectoryid",
                table: "Revenues");

            migrationBuilder.DropColumn(
                name: "propertyDirectoryid",
                table: "Revenues");

            migrationBuilder.AddColumn<string>(
                name: "propertyid",
                table: "Revenues",
                type: "nvarchar(36)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tenantid",
                table: "Revenues",
                type: "nvarchar(36)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Revenues_propertyid",
                table: "Revenues",
                column: "propertyid");

            migrationBuilder.CreateIndex(
                name: "IX_Revenues_tenantid",
                table: "Revenues",
                column: "tenantid");

            migrationBuilder.AddForeignKey(
                name: "FK_Revenues_Properties_propertyid",
                table: "Revenues",
                column: "propertyid",
                principalTable: "Properties",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Revenues_Tenants_tenantid",
                table: "Revenues",
                column: "tenantid",
                principalTable: "Tenants",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
