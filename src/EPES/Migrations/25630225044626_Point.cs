using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Migrations
{
    public partial class Point : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasSub",
                table: "PointOfEvaluations",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasSub",
                table: "PointOfEvaluations");
        }
    }
}
