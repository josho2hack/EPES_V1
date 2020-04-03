using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Migrations
{
    public partial class UpdateScore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ScoreValue",
                table: "Scores",
                newName: "Value");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Scores",
                newName: "ScoreValue");
        }
    }
}
