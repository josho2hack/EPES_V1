using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Data.Migrations
{
    public partial class POEUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "PointOfEvaluations");

            migrationBuilder.AddColumn<int>(
                name: "Plan",
                table: "PointOfEvaluations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Plan",
                table: "PointOfEvaluations");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "PointOfEvaluations",
                nullable: true);
        }
    }
}
