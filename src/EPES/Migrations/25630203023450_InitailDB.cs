using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Migrations
{
    public partial class InitailDB : Migration
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
                    Title = table.Column<string>(nullable: true),
                    FName = table.Column<string>(nullable: true),
                    LName = table.Column<string>(nullable: true),
                    PosName = table.Column<string>(nullable: true),
                    OfficeId = table.Column<string>(nullable: true),
                    OfficeName = table.Column<string>(nullable: true),
                    PIN = table.Column<string>(nullable: true),
                    Class = table.Column<string>(nullable: true),
                    GroupName = table.Column<string>(nullable: true),
                    DOB = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
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
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
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
                name: "Offices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 8, nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    ApplicationUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offices_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PointOfEvaluations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Year = table.Column<DateTime>(nullable: false),
                    Point = table.Column<int>(nullable: false),
                    SubPoint = table.Column<int>(nullable: false, defaultValue: 0),
                    Plan = table.Column<int>(nullable: false),
                    DetailPlan = table.Column<string>(nullable: true),
                    ExpectPlan = table.Column<int>(nullable: true),
                    Ddrive = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Unit = table.Column<int>(nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(7, 4)", nullable: false),
                    OwnerOfficeId = table.Column<int>(nullable: false),
                    AuditOfficeId = table.Column<int>(nullable: false),
                    UpdateUserId = table.Column<string>(nullable: true),
                    AutoApp = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointOfEvaluations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointOfEvaluations_Offices_AuditOfficeId",
                        column: x => x.AuditOfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PointOfEvaluations_Offices_OwnerOfficeId",
                        column: x => x.OwnerOfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PointOfEvaluations_AspNetUsers_UpdateUserId",
                        column: x => x.UpdateUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DataForEvaluations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Expect = table.Column<decimal>(type: "decimal(38, 10)", nullable: false, defaultValue: 0m),
                    Result = table.Column<decimal>(type: "decimal(38, 10)", nullable: false, defaultValue: 0m),
                    OldResult = table.Column<decimal>(type: "decimal(38, 10)", nullable: true),
                    Month = table.Column<int>(nullable: false),
                    Approve = table.Column<int>(nullable: false),
                    CommentApproveLevel1 = table.Column<string>(nullable: true),
                    CommentApproveLevel2 = table.Column<string>(nullable: true),
                    CommentApproveLevel3 = table.Column<string>(nullable: true),
                    CommentApproveLevel4 = table.Column<string>(nullable: true),
                    CompletedDate = table.Column<DateTime>(nullable: true),
                    AttachFile = table.Column<string>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    TimeUpdate = table.Column<DateTime>(nullable: false),
                    UpdateUserId = table.Column<string>(nullable: true),
                    OfficeId = table.Column<int>(nullable: false),
                    PointOfEvaluationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataForEvaluations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataForEvaluations_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DataForEvaluations_PointOfEvaluations_PointOfEvaluationId",
                        column: x => x.PointOfEvaluationId,
                        principalTable: "PointOfEvaluations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DataForEvaluations_AspNetUsers_UpdateUserId",
                        column: x => x.UpdateUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rounds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoundNumber = table.Column<int>(nullable: false),
                    LevelNumber = table.Column<int>(nullable: true),
                    Rate1 = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    DetailRate1 = table.Column<string>(nullable: true),
                    Rate1MonthStart = table.Column<DateTime>(nullable: true),
                    Rate1MonthStop = table.Column<DateTime>(nullable: true),
                    Rate2 = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    DetailRate2 = table.Column<string>(nullable: true),
                    Rate2MonthStart = table.Column<DateTime>(nullable: true),
                    Rate2MonthStop = table.Column<DateTime>(nullable: true),
                    Rate3 = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    DetailRate3 = table.Column<string>(nullable: true),
                    Rate3MonthStart = table.Column<DateTime>(nullable: true),
                    Rate3MonthStop = table.Column<DateTime>(nullable: true),
                    Rate4 = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    DetailRate4 = table.Column<string>(nullable: true),
                    Rate4MonthStart = table.Column<DateTime>(nullable: true),
                    Rate4MonthStop = table.Column<DateTime>(nullable: true),
                    Rate5 = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    DetailRate5 = table.Column<string>(nullable: true),
                    Rate5MonthStart = table.Column<DateTime>(nullable: true),
                    Rate5MonthStop = table.Column<DateTime>(nullable: true),
                    PointOfEvaluationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rounds_PointOfEvaluations_PointOfEvaluationId",
                        column: x => x.PointOfEvaluationId,
                        principalTable: "PointOfEvaluations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScoreDrafts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ScoreValue = table.Column<decimal>(type: "decimal(5, 4)", nullable: false),
                    LastMonth = table.Column<int>(nullable: false),
                    PointOfEvaluationId = table.Column<int>(nullable: false),
                    OfficeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreDrafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoreDrafts_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreDrafts_PointOfEvaluations_PointOfEvaluationId",
                        column: x => x.PointOfEvaluationId,
                        principalTable: "PointOfEvaluations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ScoreValue = table.Column<decimal>(type: "decimal(5, 4)", nullable: false),
                    LastMonth = table.Column<int>(nullable: false),
                    PointOfEvaluationId = table.Column<int>(nullable: false),
                    OfficeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scores_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Scores_PointOfEvaluations_PointOfEvaluationId",
                        column: x => x.PointOfEvaluationId,
                        principalTable: "PointOfEvaluations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_DataForEvaluations_OfficeId",
                table: "DataForEvaluations",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_DataForEvaluations_PointOfEvaluationId",
                table: "DataForEvaluations",
                column: "PointOfEvaluationId");

            migrationBuilder.CreateIndex(
                name: "IX_DataForEvaluations_UpdateUserId",
                table: "DataForEvaluations",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Offices_ApplicationUserId",
                table: "Offices",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PointOfEvaluations_AuditOfficeId",
                table: "PointOfEvaluations",
                column: "AuditOfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_PointOfEvaluations_OwnerOfficeId",
                table: "PointOfEvaluations",
                column: "OwnerOfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_PointOfEvaluations_UpdateUserId",
                table: "PointOfEvaluations",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_PointOfEvaluationId",
                table: "Rounds",
                column: "PointOfEvaluationId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreDrafts_OfficeId",
                table: "ScoreDrafts",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreDrafts_PointOfEvaluationId",
                table: "ScoreDrafts",
                column: "PointOfEvaluationId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_OfficeId",
                table: "Scores",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_PointOfEvaluationId",
                table: "Scores",
                column: "PointOfEvaluationId");
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
                name: "DataForEvaluations");

            migrationBuilder.DropTable(
                name: "Rounds");

            migrationBuilder.DropTable(
                name: "ScoreDrafts");

            migrationBuilder.DropTable(
                name: "Scores");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "PointOfEvaluations");

            migrationBuilder.DropTable(
                name: "Offices");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
