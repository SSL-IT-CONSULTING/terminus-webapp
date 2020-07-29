using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus.dataaccess.Migrations
{
    public partial class dbchanges20200727c : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CCTNumber",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "MgtFeePct",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "Owned_Mgd",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "emergy_FullName",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "otherResFullName1",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "otherResFullName2",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "otherResFullName3",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "otherResRelationshipToOwner1",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "otherResRelationshipToOwner2",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "otherResRelationshipToOwner3",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "otherResResiding1",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "otherResResiding2",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "otherResResiding3",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "otherRestenanted1",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "otherRestenanted2",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "otherRestenanted3",
                table: "Tenants");

            migrationBuilder.AddColumn<string>(
                name: "emergyFullName",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "homeAddress",
                table: "Tenants",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CCTNumber",
                table: "Properties",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MgtFeePct",
                table: "Properties",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Owned_Mgd",
                table: "Properties",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "emergyAdrress",
                table: "Properties",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "emergyContactNo",
                table: "Properties",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "emergyFullName",
                table: "Properties",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "emergyRelationshipOwner",
                table: "Properties",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otherResFullName1",
                table: "Properties",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otherResFullName2",
                table: "Properties",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otherResFullName3",
                table: "Properties",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otherResRelationshipToOwner1",
                table: "Properties",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otherResRelationshipToOwner2",
                table: "Properties",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otherResRelationshipToOwner3",
                table: "Properties",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "otherResResiding",
                table: "Properties",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "otherRestenanted",
                table: "Properties",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "emergyFullName",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "homeAddress",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "CCTNumber",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "MgtFeePct",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Owned_Mgd",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "emergyAdrress",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "emergyContactNo",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "emergyFullName",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "emergyRelationshipOwner",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "otherResFullName1",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "otherResFullName2",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "otherResFullName3",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "otherResRelationshipToOwner1",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "otherResRelationshipToOwner2",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "otherResRelationshipToOwner3",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "otherResResiding",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "otherRestenanted",
                table: "Properties");

            migrationBuilder.AddColumn<string>(
                name: "CCTNumber",
                table: "Tenants",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MgtFeePct",
                table: "Tenants",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Owned_Mgd",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "emergy_FullName",
                table: "Tenants",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otherResFullName1",
                table: "Tenants",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otherResFullName2",
                table: "Tenants",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otherResFullName3",
                table: "Tenants",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otherResRelationshipToOwner1",
                table: "Tenants",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otherResRelationshipToOwner2",
                table: "Tenants",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otherResRelationshipToOwner3",
                table: "Tenants",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "otherResResiding1",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "otherResResiding2",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "otherResResiding3",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "otherRestenanted1",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "otherRestenanted2",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "otherRestenanted3",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
