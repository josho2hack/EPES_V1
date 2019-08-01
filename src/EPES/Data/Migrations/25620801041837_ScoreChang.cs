using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Data.Migrations
{
    public partial class ScoreChang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "ScoresDrafts",
                type: "decimal(5, 4)",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "Scores",
                type: "decimal(5, 4)",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "ScoresDrafts",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 4)");

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "Scores",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 4)");
        }
    }
}
