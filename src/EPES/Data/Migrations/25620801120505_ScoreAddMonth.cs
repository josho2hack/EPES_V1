using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Data.Migrations
{
    public partial class ScoreAddMonth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastMonth",
                table: "Scores",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastMonth",
                table: "ScoreDrafts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastMonth",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "LastMonth",
                table: "ScoreDrafts");
        }
    }
}
