using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Migrations
{
    public partial class UpdateScoreDraftApprove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScoreDrafts_Scores_ScoreId",
                table: "ScoreDrafts");

            migrationBuilder.DropIndex(
                name: "IX_ScoreDrafts_ScoreId",
                table: "ScoreDrafts");

            migrationBuilder.DropColumn(
                name: "ScoreId",
                table: "ScoreDrafts");

            migrationBuilder.AddColumn<decimal>(
                name: "ScoreApprove",
                table: "ScoreDrafts",
                type: "decimal(5, 4)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScoreApprove",
                table: "ScoreDrafts");

            migrationBuilder.AddColumn<int>(
                name: "ScoreId",
                table: "ScoreDrafts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScoreDrafts_ScoreId",
                table: "ScoreDrafts",
                column: "ScoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScoreDrafts_Scores_ScoreId",
                table: "ScoreDrafts",
                column: "ScoreId",
                principalTable: "Scores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
