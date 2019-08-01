using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Data.Migrations
{
    public partial class ScoreChangId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Offices_OfficeID",
                table: "Scores");

            migrationBuilder.DropForeignKey(
                name: "FK_Scores_PointOfEvaluations_PointOfEvaluationID",
                table: "Scores");

            migrationBuilder.DropForeignKey(
                name: "FK_ScoresDrafts_Offices_OfficeID",
                table: "ScoresDrafts");

            migrationBuilder.DropForeignKey(
                name: "FK_ScoresDrafts_PointOfEvaluations_PointOfEvaluationID",
                table: "ScoresDrafts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScoresDrafts",
                table: "ScoresDrafts");

            migrationBuilder.RenameTable(
                name: "ScoresDrafts",
                newName: "ScoreDrafts");

            migrationBuilder.RenameColumn(
                name: "PointOfEvaluationID",
                table: "Scores",
                newName: "PointOfEvaluationId");

            migrationBuilder.RenameColumn(
                name: "OfficeID",
                table: "Scores",
                newName: "OfficeId");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Scores",
                newName: "ScoreValue");

            migrationBuilder.RenameIndex(
                name: "IX_Scores_PointOfEvaluationID",
                table: "Scores",
                newName: "IX_Scores_PointOfEvaluationId");

            migrationBuilder.RenameIndex(
                name: "IX_Scores_OfficeID",
                table: "Scores",
                newName: "IX_Scores_OfficeId");

            migrationBuilder.RenameColumn(
                name: "PointOfEvaluationID",
                table: "ScoreDrafts",
                newName: "PointOfEvaluationId");

            migrationBuilder.RenameColumn(
                name: "OfficeID",
                table: "ScoreDrafts",
                newName: "OfficeId");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "ScoreDrafts",
                newName: "ScoreValue");

            migrationBuilder.RenameIndex(
                name: "IX_ScoresDrafts_PointOfEvaluationID",
                table: "ScoreDrafts",
                newName: "IX_ScoreDrafts_PointOfEvaluationId");

            migrationBuilder.RenameIndex(
                name: "IX_ScoresDrafts_OfficeID",
                table: "ScoreDrafts",
                newName: "IX_ScoreDrafts_OfficeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScoreDrafts",
                table: "ScoreDrafts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ScoreDrafts_Offices_OfficeId",
                table: "ScoreDrafts",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScoreDrafts_PointOfEvaluations_PointOfEvaluationId",
                table: "ScoreDrafts",
                column: "PointOfEvaluationId",
                principalTable: "PointOfEvaluations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Offices_OfficeId",
                table: "Scores",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_PointOfEvaluations_PointOfEvaluationId",
                table: "Scores",
                column: "PointOfEvaluationId",
                principalTable: "PointOfEvaluations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScoreDrafts_Offices_OfficeId",
                table: "ScoreDrafts");

            migrationBuilder.DropForeignKey(
                name: "FK_ScoreDrafts_PointOfEvaluations_PointOfEvaluationId",
                table: "ScoreDrafts");

            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Offices_OfficeId",
                table: "Scores");

            migrationBuilder.DropForeignKey(
                name: "FK_Scores_PointOfEvaluations_PointOfEvaluationId",
                table: "Scores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ScoreDrafts",
                table: "ScoreDrafts");

            migrationBuilder.RenameTable(
                name: "ScoreDrafts",
                newName: "ScoresDrafts");

            migrationBuilder.RenameColumn(
                name: "PointOfEvaluationId",
                table: "Scores",
                newName: "PointOfEvaluationID");

            migrationBuilder.RenameColumn(
                name: "OfficeId",
                table: "Scores",
                newName: "OfficeID");

            migrationBuilder.RenameColumn(
                name: "ScoreValue",
                table: "Scores",
                newName: "Value");

            migrationBuilder.RenameIndex(
                name: "IX_Scores_PointOfEvaluationId",
                table: "Scores",
                newName: "IX_Scores_PointOfEvaluationID");

            migrationBuilder.RenameIndex(
                name: "IX_Scores_OfficeId",
                table: "Scores",
                newName: "IX_Scores_OfficeID");

            migrationBuilder.RenameColumn(
                name: "PointOfEvaluationId",
                table: "ScoresDrafts",
                newName: "PointOfEvaluationID");

            migrationBuilder.RenameColumn(
                name: "OfficeId",
                table: "ScoresDrafts",
                newName: "OfficeID");

            migrationBuilder.RenameColumn(
                name: "ScoreValue",
                table: "ScoresDrafts",
                newName: "Value");

            migrationBuilder.RenameIndex(
                name: "IX_ScoreDrafts_PointOfEvaluationId",
                table: "ScoresDrafts",
                newName: "IX_ScoresDrafts_PointOfEvaluationID");

            migrationBuilder.RenameIndex(
                name: "IX_ScoreDrafts_OfficeId",
                table: "ScoresDrafts",
                newName: "IX_ScoresDrafts_OfficeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScoresDrafts",
                table: "ScoresDrafts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Offices_OfficeID",
                table: "Scores",
                column: "OfficeID",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_PointOfEvaluations_PointOfEvaluationID",
                table: "Scores",
                column: "PointOfEvaluationID",
                principalTable: "PointOfEvaluations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScoresDrafts_Offices_OfficeID",
                table: "ScoresDrafts",
                column: "OfficeID",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScoresDrafts_PointOfEvaluations_PointOfEvaluationID",
                table: "ScoresDrafts",
                column: "PointOfEvaluationID",
                principalTable: "PointOfEvaluations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
