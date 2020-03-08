using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus_webapp.Migrations
{
    public partial class dbchange38 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<string>(maxLength: 36, nullable: false),
                    createdBy = table.Column<string>(maxLength: 100, nullable: true),
                    createDate = table.Column<DateTime>(nullable: false),
                    updatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    updateDate = table.Column<DateTime>(nullable: true),
                    deleted = table.Column<bool>(nullable: false),
                    CompanyId = table.Column<string>(maxLength: 10, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    HireDate = table.Column<DateTime>(nullable: false),
                    LastName = table.Column<string>(maxLength: 70, nullable: true),
                    FirstName = table.Column<string>(maxLength: 70, nullable: true),
                    MiddleName = table.Column<string>(maxLength: 70, nullable: true),
                    Position = table.Column<string>(maxLength: 50, nullable: true),
                    Address = table.Column<string>(maxLength: 708, nullable: true),
                    ContactNo = table.Column<string>(maxLength: 20, nullable: true),
                    Email = table.Column<string>(maxLength: 500, nullable: true),
                    SSS = table.Column<string>(maxLength: 50, nullable: true),
                    PhilHealth = table.Column<string>(maxLength: 50, nullable: true),
                    PagIbig = table.Column<string>(maxLength: 50, nullable: true),
                    TIN = table.Column<string>(maxLength: 50, nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: true),
                    Gender = table.Column<string>(maxLength: 10, nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    Remarks = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employees_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyId",
                table: "Employees",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
