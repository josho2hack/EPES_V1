using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Migrations
{
    public partial class AddWeightDataForEvaluation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "DataForEvaluations",
                type: "decimal(7, 4)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                table: "DataForEvaluations");
        }
    }
}
