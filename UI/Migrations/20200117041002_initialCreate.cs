using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace terminus_webapp.Migrations
{
    public partial class initialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CheckDetails",
                columns: table => new
                {
                    checkDetailId = table.Column<Guid>(nullable: false),
                    bankName = table.Column<string>(maxLength: 300, nullable: true),
                    branch = table.Column<string>(maxLength: 300, nullable: true),
                    checkDate = table.Column<DateTime>(nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckDetails", x => x.checkDetailId);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    companyId = table.Column<string>(maxLength: 10, nullable: false),
                    companyName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.companyId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    firstName = table.Column<string>(maxLength: 100, nullable: false),
                    lastName = table.Column<string>(maxLength: 100, nullable: false),
                    middleName = table.Column<string>(maxLength: 100, nullable: true),
                    companyId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Companies_companyId",
                        column: x => x.companyId,
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GLAccounts",
                columns: table => new
                {
                    accountId = table.Column<Guid>(nullable: false),
                    createdBy = table.Column<string>(maxLength: 100, nullable: true),
                    createDate = table.Column<DateTime>(nullable: false),
                    updatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    updateDate = table.Column<DateTime>(nullable: false),
                    deleted = table.Column<bool>(nullable: false),
                    accountCode = table.Column<string>(maxLength: 120, nullable: true),
                    accountDesc = table.Column<string>(maxLength: 1000, nullable: true),
                    balance = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    companyId = table.Column<string>(nullable: true),
                    revenue = table.Column<bool>(nullable: false),
                    expense = table.Column<bool>(nullable: false),
                    cashAccount = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GLAccounts", x => x.accountId);
                    table.ForeignKey(
                        name: "FK_GLAccounts_Companies_companyId",
                        column: x => x.companyId,
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntriesHdr",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    createdBy = table.Column<string>(maxLength: 100, nullable: true),
                    createDate = table.Column<DateTime>(nullable: false),
                    updatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    updateDate = table.Column<DateTime>(nullable: false),
                    deleted = table.Column<bool>(nullable: false),
                    description = table.Column<string>(maxLength: 120, nullable: true),
                    companyId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntriesHdr", x => x.id);
                    table.ForeignKey(
                        name: "FK_JournalEntriesHdr_Companies_companyId",
                        column: x => x.companyId,
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 36, nullable: false),
                    createdBy = table.Column<string>(maxLength: 100, nullable: true),
                    createDate = table.Column<DateTime>(nullable: false),
                    updatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    updateDate = table.Column<DateTime>(nullable: false),
                    deleted = table.Column<bool>(nullable: false),
                    companyId = table.Column<string>(nullable: true),
                    lastName = table.Column<string>(maxLength: 100, nullable: true),
                    firstName = table.Column<string>(maxLength: 100, nullable: true),
                    middleName = table.Column<string>(maxLength: 100, nullable: true),
                    contactNumber = table.Column<string>(maxLength: 20, nullable: true),
                    emailAddress = table.Column<string>(maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.id);
                    table.ForeignKey(
                        name: "FK_Owners_Companies_companyId",
                        column: x => x.companyId,
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 36, nullable: false),
                    createdBy = table.Column<string>(maxLength: 100, nullable: true),
                    createDate = table.Column<DateTime>(nullable: false),
                    updatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    updateDate = table.Column<DateTime>(nullable: false),
                    deleted = table.Column<bool>(nullable: false),
                    companyId = table.Column<string>(nullable: true),
                    description = table.Column<string>(maxLength: 100, nullable: true),
                    address = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.id);
                    table.ForeignKey(
                        name: "FK_Properties_Companies_companyId",
                        column: x => x.companyId,
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 36, nullable: false),
                    createdBy = table.Column<string>(maxLength: 100, nullable: true),
                    createDate = table.Column<DateTime>(nullable: false),
                    updatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    updateDate = table.Column<DateTime>(nullable: false),
                    deleted = table.Column<bool>(nullable: false),
                    companyId = table.Column<string>(nullable: true),
                    lastName = table.Column<string>(maxLength: 100, nullable: true),
                    firstName = table.Column<string>(maxLength: 100, nullable: true),
                    middleName = table.Column<string>(maxLength: 100, nullable: true),
                    contactNumber = table.Column<string>(maxLength: 20, nullable: true),
                    emailAddress = table.Column<string>(maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.id);
                    table.ForeignKey(
                        name: "FK_Tenants_Companies_companyId",
                        column: x => x.companyId,
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    createdBy = table.Column<string>(maxLength: 100, nullable: true),
                    createDate = table.Column<DateTime>(nullable: false),
                    updatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    updateDate = table.Column<DateTime>(nullable: false),
                    deleted = table.Column<bool>(nullable: false),
                    companyId = table.Column<string>(nullable: true),
                    accountId = table.Column<Guid>(nullable: true),
                    transactionDate = table.Column<DateTime>(nullable: false),
                    description = table.Column<string>(maxLength: 200, nullable: true),
                    remarks = table.Column<string>(maxLength: 500, nullable: true),
                    amount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    reference = table.Column<string>(maxLength: 100, nullable: true),
                    cashOrCheck = table.Column<bool>(nullable: false),
                    checkDetailscheckDetailId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.id);
                    table.ForeignKey(
                        name: "FK_Expenses_GLAccounts_accountId",
                        column: x => x.accountId,
                        principalTable: "GLAccounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Expenses_CheckDetails_checkDetailscheckDetailId",
                        column: x => x.checkDetailscheckDetailId,
                        principalTable: "CheckDetails",
                        principalColumn: "checkDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Expenses_Companies_companyId",
                        column: x => x.companyId,
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntriesDtl",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 36, nullable: false),
                    createdBy = table.Column<string>(maxLength: 100, nullable: true),
                    createDate = table.Column<DateTime>(nullable: false),
                    updatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    updateDate = table.Column<DateTime>(nullable: false),
                    deleted = table.Column<bool>(nullable: false),
                    amount = table.Column<decimal>(type: "decimal(14,4)", nullable: false),
                    type = table.Column<string>(maxLength: 1, nullable: true),
                    lineNumber = table.Column<int>(nullable: false),
                    JournalEntryHdrid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntriesDtl", x => x.id);
                    table.ForeignKey(
                        name: "FK_JournalEntriesDtl_JournalEntriesHdr_JournalEntryHdrid",
                        column: x => x.JournalEntryHdrid,
                        principalTable: "JournalEntriesHdr",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PropertyDirectory",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    createdBy = table.Column<string>(maxLength: 100, nullable: true),
                    createDate = table.Column<DateTime>(nullable: false),
                    updatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    updateDate = table.Column<DateTime>(nullable: false),
                    deleted = table.Column<bool>(nullable: false),
                    propertyid = table.Column<string>(nullable: true),
                    tenantid = table.Column<string>(nullable: true),
                    dateFrom = table.Column<DateTime>(nullable: false),
                    dateTo = table.Column<DateTime>(nullable: false),
                    companyId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyDirectory", x => x.id);
                    table.ForeignKey(
                        name: "FK_PropertyDirectory_Companies_companyId",
                        column: x => x.companyId,
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropertyDirectory_Properties_propertyid",
                        column: x => x.propertyid,
                        principalTable: "Properties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropertyDirectory_Tenants_tenantid",
                        column: x => x.tenantid,
                        principalTable: "Tenants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Revenues",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    createdBy = table.Column<string>(maxLength: 100, nullable: true),
                    createDate = table.Column<DateTime>(nullable: false),
                    updatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    updateDate = table.Column<DateTime>(nullable: false),
                    deleted = table.Column<bool>(nullable: false),
                    companyId = table.Column<string>(nullable: true),
                    accountId = table.Column<Guid>(nullable: true),
                    transactionDate = table.Column<DateTime>(nullable: false),
                    description = table.Column<string>(maxLength: 200, nullable: true),
                    remarks = table.Column<string>(maxLength: 500, nullable: true),
                    amount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    reference = table.Column<string>(maxLength: 100, nullable: true),
                    cashOrCheck = table.Column<bool>(nullable: false),
                    checkDetailscheckDetailId = table.Column<Guid>(nullable: true),
                    propertyid = table.Column<string>(nullable: true),
                    tenantid = table.Column<string>(nullable: true),
                    dueDate = table.Column<DateTime>(nullable: true),
                    cashAccountaccountId = table.Column<Guid>(nullable: true),
                    journalEntryid = table.Column<Guid>(nullable: true),
                    receiptNo = table.Column<string>(maxLength: 36, nullable: true),
                    taxAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Revenues", x => x.id);
                    table.ForeignKey(
                        name: "FK_Revenues_GLAccounts_accountId",
                        column: x => x.accountId,
                        principalTable: "GLAccounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Revenues_GLAccounts_cashAccountaccountId",
                        column: x => x.cashAccountaccountId,
                        principalTable: "GLAccounts",
                        principalColumn: "accountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Revenues_CheckDetails_checkDetailscheckDetailId",
                        column: x => x.checkDetailscheckDetailId,
                        principalTable: "CheckDetails",
                        principalColumn: "checkDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Revenues_Companies_companyId",
                        column: x => x.companyId,
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Revenues_JournalEntriesHdr_journalEntryid",
                        column: x => x.journalEntryid,
                        principalTable: "JournalEntriesHdr",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Revenues_Properties_propertyid",
                        column: x => x.propertyid,
                        principalTable: "Properties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Revenues_Tenants_tenantid",
                        column: x => x.tenantid,
                        principalTable: "Tenants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_companyId",
                table: "AspNetUsers",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_accountId",
                table: "Expenses",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_checkDetailscheckDetailId",
                table: "Expenses",
                column: "checkDetailscheckDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_companyId",
                table: "Expenses",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_GLAccounts_companyId",
                table: "GLAccounts",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntriesDtl_JournalEntryHdrid",
                table: "JournalEntriesDtl",
                column: "JournalEntryHdrid");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntriesHdr_companyId",
                table: "JournalEntriesHdr",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_companyId",
                table: "Owners",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_companyId",
                table: "Properties",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDirectory_companyId",
                table: "PropertyDirectory",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDirectory_propertyid",
                table: "PropertyDirectory",
                column: "propertyid");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDirectory_tenantid",
                table: "PropertyDirectory",
                column: "tenantid");

            migrationBuilder.CreateIndex(
                name: "IX_Revenues_accountId",
                table: "Revenues",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_Revenues_cashAccountaccountId",
                table: "Revenues",
                column: "cashAccountaccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Revenues_checkDetailscheckDetailId",
                table: "Revenues",
                column: "checkDetailscheckDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Revenues_companyId",
                table: "Revenues",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_Revenues_journalEntryid",
                table: "Revenues",
                column: "journalEntryid");

            migrationBuilder.CreateIndex(
                name: "IX_Revenues_propertyid",
                table: "Revenues",
                column: "propertyid");

            migrationBuilder.CreateIndex(
                name: "IX_Revenues_tenantid",
                table: "Revenues",
                column: "tenantid");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_companyId",
                table: "Tenants",
                column: "companyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "JournalEntriesDtl");

            migrationBuilder.DropTable(
                name: "Owners");

            migrationBuilder.DropTable(
                name: "PropertyDirectory");

            migrationBuilder.DropTable(
                name: "Revenues");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "GLAccounts");

            migrationBuilder.DropTable(
                name: "CheckDetails");

            migrationBuilder.DropTable(
                name: "JournalEntriesHdr");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
