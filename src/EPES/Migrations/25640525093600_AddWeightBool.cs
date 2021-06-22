using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Migrations
{
    public partial class AddWeightBool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WeightAll",
                table: "PointOfEvaluations",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeightAll",
                table: "PointOfEvaluations");
        }
    }
}
