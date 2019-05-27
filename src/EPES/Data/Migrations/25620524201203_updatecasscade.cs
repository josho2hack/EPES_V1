using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Data.Migrations
{
    public partial class updatecasscade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataForEvaluations_PointOfEvaluations_PointOfEvaluationId",
                table: "DataForEvaluations");

            migrationBuilder.AddForeignKey(
                name: "FK_DataForEvaluations_PointOfEvaluations_PointOfEvaluationId",
                table: "DataForEvaluations",
                column: "PointOfEvaluationId",
                principalTable: "PointOfEvaluations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataForEvaluations_PointOfEvaluations_PointOfEvaluationId",
                table: "DataForEvaluations");

            migrationBuilder.AddForeignKey(
                name: "FK_DataForEvaluations_PointOfEvaluations_PointOfEvaluationId",
                table: "DataForEvaluations",
                column: "PointOfEvaluationId",
                principalTable: "PointOfEvaluations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
