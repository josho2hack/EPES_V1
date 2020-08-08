using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Migrations
{
    public partial class addFixAndCalpermonthPOE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CalPerMonth",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FixExpect",
                table: "PointOfEvaluations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalPerMonth",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "FixExpect",
                table: "PointOfEvaluations");
        }
    }
}
