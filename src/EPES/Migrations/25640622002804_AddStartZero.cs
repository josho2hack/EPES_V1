using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Migrations
{
    public partial class AddStartZero : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "StartZero",
                table: "PointOfEvaluations",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartZero",
                table: "PointOfEvaluations");
        }
    }
}
