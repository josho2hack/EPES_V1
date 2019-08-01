using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Data.Migrations
{
    public partial class ScoreDaft : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScoresDrafts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<int>(nullable: false),
                    LastMonth = table.Column<DateTime>(nullable: false),
                    PointOfEvaluationID = table.Column<int>(nullable: false),
                    OfficeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoresDrafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoresDrafts_Offices_OfficeID",
                        column: x => x.OfficeID,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoresDrafts_PointOfEvaluations_PointOfEvaluationID",
                        column: x => x.PointOfEvaluationID,
                        principalTable: "PointOfEvaluations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScoresDrafts_OfficeID",
                table: "ScoresDrafts",
                column: "OfficeID");

            migrationBuilder.CreateIndex(
                name: "IX_ScoresDrafts_PointOfEvaluationID",
                table: "ScoresDrafts",
                column: "PointOfEvaluationID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScoresDrafts");
        }
    }
}
