using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Data.Migrations
{
    public partial class deleteAapNetLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataForEvaluations_AspNetUsers_UpdateUserId",
                table: "DataForEvaluations");

            migrationBuilder.DropForeignKey(
                name: "FK_PointOfEvaluations_AspNetUsers_UpdateUserId",
                table: "PointOfEvaluations");

            migrationBuilder.DropIndex(
                name: "IX_PointOfEvaluations_UpdateUserId",
                table: "PointOfEvaluations");

            migrationBuilder.DropIndex(
                name: "IX_DataForEvaluations_UpdateUserId",
                table: "DataForEvaluations");

            migrationBuilder.DropColumn(
                name: "UpdateUserId",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "UpdateUserId",
                table: "DataForEvaluations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UpdateUserId",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateUserId",
                table: "DataForEvaluations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PointOfEvaluations_UpdateUserId",
                table: "PointOfEvaluations",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DataForEvaluations_UpdateUserId",
                table: "DataForEvaluations",
                column: "UpdateUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DataForEvaluations_AspNetUsers_UpdateUserId",
                table: "DataForEvaluations",
                column: "UpdateUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PointOfEvaluations_AspNetUsers_UpdateUserId",
                table: "PointOfEvaluations",
                column: "UpdateUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
