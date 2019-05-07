using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Data.Migrations
{
    public partial class UpdateModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "PointOfEvaluations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "UpdateUserId",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DataForEvaluations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Expect = table.Column<decimal>(type: "decimal(38, 10)", nullable: false),
                    Result = table.Column<decimal>(type: "decimal(38, 10)", nullable: false),
                    OldResult = table.Column<decimal>(type: "decimal(38, 10)", nullable: false),
                    Month = table.Column<DateTime>(nullable: false),
                    AuditComment = table.Column<string>(nullable: true),
                    Approve = table.Column<int>(nullable: true),
                    CommentApprove = table.Column<string>(nullable: true),
                    PointOfEvaluationId = table.Column<int>(nullable: true),
                    OfficeId = table.Column<int>(nullable: true),
                    UpdateUserId = table.Column<string>(nullable: true)
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
                name: "Scores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<int>(nullable: false),
                    LastMonth = table.Column<DateTime>(nullable: false),
                    PointOfEvaluationId = table.Column<int>(nullable: true),
                    OfficeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scores_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scores_PointOfEvaluations_PointOfEvaluationId",
                        column: x => x.PointOfEvaluationId,
                        principalTable: "PointOfEvaluations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointOfEvaluations_UpdateUserId",
                table: "PointOfEvaluations",
                column: "UpdateUserId");

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
                name: "IX_Scores_OfficeId",
                table: "Scores",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_PointOfEvaluationId",
                table: "Scores",
                column: "PointOfEvaluationId");

            migrationBuilder.AddForeignKey(
                name: "FK_PointOfEvaluations_AspNetUsers_UpdateUserId",
                table: "PointOfEvaluations",
                column: "UpdateUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PointOfEvaluations_AspNetUsers_UpdateUserId",
                table: "PointOfEvaluations");

            migrationBuilder.DropTable(
                name: "DataForEvaluations");

            migrationBuilder.DropTable(
                name: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_PointOfEvaluations_UpdateUserId",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "UpdateUserId",
                table: "PointOfEvaluations");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "PointOfEvaluations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
