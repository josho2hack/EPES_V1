using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Data.Migrations
{
    public partial class updatecascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataForEvaluations_Offices_OfficeId",
                table: "DataForEvaluations");

            migrationBuilder.AddForeignKey(
                name: "FK_DataForEvaluations_Offices_OfficeId",
                table: "DataForEvaluations",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataForEvaluations_Offices_OfficeId",
                table: "DataForEvaluations");

            migrationBuilder.AddForeignKey(
                name: "FK_DataForEvaluations_Offices_OfficeId",
                table: "DataForEvaluations",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
