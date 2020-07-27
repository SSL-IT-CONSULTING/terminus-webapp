using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus.dataaccess.Migrations
{
    public partial class dbchanges20200726 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CCTNumber",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MgtFeePct",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Owned_Mgd",
                table: "Tenants",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelToPrimary1",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelToPrimary2",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelToPrimary3",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "contactNo2",
                table: "Tenants",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "contactNo3",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "emergyAdrress",
                table: "Tenants",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "emergyContactNo",
                table: "Tenants",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "emergyRelationshipOwner",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "emergy_FullName",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otherResFullName1",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otherResFullName2",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otherResFullName3",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otherResRelationshipToOwner1",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otherResRelationshipToOwner2",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "otherResRelationshipToOwner3",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "otherResResiding1",
                table: "Tenants",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "otherResResiding2",
                table: "Tenants",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "otherResResiding3",
                table: "Tenants",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "otherRestenanted1",
                table: "Tenants",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "otherRestenanted2",
                table: "Tenants",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "otherRestenanted3",
                table: "Tenants",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "subTenantContactNo1",
                table: "Tenants",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "subTenantContactNo2",
                table: "Tenants",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "subTenantContactNo3",
                table: "Tenants",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "subTenantEmailAdd1",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "subTenantEmailAdd2",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "subTenantEmailAdd3",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "subTenantFullName1",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "subTenantFullName2",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "subTenantFullName3",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "subTenantHomeAddress1",
                table: "Tenants",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "subTenantHomeAddress2",
                table: "Tenants",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "subTenantHomeAddress3",
                table: "Tenants",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "subTenantID1",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "subTenantID2",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "subTenantID3",
                table: "Tenants",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "subTenantWorkAddress1",
                table: "Tenants",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "subTenantWorkAddress2",
                table: "Tenants",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "subTenantWorkAddress3",
                table: "Tenants",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "workAddress",
                table: "Tenants",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "RelToPrimary1",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "RelToPrimary2",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "RelToPrimary3",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "contactNo2",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "contactNo3",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "emergyAdrress",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "emergyContactNo",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "emergyRelationshipOwner",
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

            migrationBuilder.DropColumn(
                name: "subTenantContactNo1",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "subTenantContactNo2",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "subTenantContactNo3",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "subTenantEmailAdd1",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "subTenantEmailAdd2",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "subTenantEmailAdd3",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "subTenantFullName1",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "subTenantFullName2",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "subTenantFullName3",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "subTenantHomeAddress1",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "subTenantHomeAddress2",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "subTenantHomeAddress3",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "subTenantID1",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "subTenantID2",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "subTenantID3",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "subTenantWorkAddress1",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "subTenantWorkAddress2",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "subTenantWorkAddress3",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "workAddress",
                table: "Tenants");
        }
    }
}
