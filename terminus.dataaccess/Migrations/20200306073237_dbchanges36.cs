using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus.dataaccess.Migrations
{
    public partial class dbchanges36 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PropertyDocument",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    createdBy = table.Column<string>(maxLength: 100, nullable: true),
                    createDate = table.Column<DateTime>(nullable: false),
                    updatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    updateDate = table.Column<DateTime>(nullable: true),
                    deleted = table.Column<bool>(nullable: false),
                    propertyId = table.Column<Guid>(nullable: false),
                    fileName = table.Column<string>(maxLength: 500, nullable: true),
                    filePath = table.Column<string>(maxLength: 1000, nullable: true),
                    extName = table.Column<string>(maxLength: 20, nullable: true),
                    fileDesc = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyDocument", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropertyDocument");
        }
    }
}
