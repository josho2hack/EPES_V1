using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Migrations
{
    public partial class UpdateDataforPOE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ResultLevelRate",
                table: "DataForEvaluations",
                type: "decimal(38, 10)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResultLevelRate",
                table: "DataForEvaluations");
        }
    }
}
