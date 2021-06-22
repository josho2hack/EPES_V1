using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Migrations
{
    public partial class AddWeightDefault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "WeightAll",
                table: "PointOfEvaluations",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "WeightAll",
                table: "PointOfEvaluations",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: true);
        }
    }
}
