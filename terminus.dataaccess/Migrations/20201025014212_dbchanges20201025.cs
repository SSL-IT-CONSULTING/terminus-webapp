using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus.dataaccess.Migrations
{
    public partial class dbchanges20201025 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "subTenantProxID1",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "subTenantProxID2",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "subTenantProxID3",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tenantProxID",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otherResProxID1",
                table: "Properties",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otherResProxID2",
                table: "Properties",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otherResProxID3",
                table: "Properties",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ownerProxID",
                table: "Properties",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "subTenantProxID1",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "subTenantProxID2",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "subTenantProxID3",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "tenantProxID",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "otherResProxID1",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "otherResProxID2",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "otherResProxID3",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "ownerProxID",
                table: "Properties");
        }
    }
}
