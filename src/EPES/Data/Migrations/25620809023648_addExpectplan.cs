using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Data.Migrations
{
    public partial class addExpectplan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ddrive",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpectPlan",
                table: "PointOfEvaluations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ddrive",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "ExpectPlan",
                table: "PointOfEvaluations");
        }
    }
}
