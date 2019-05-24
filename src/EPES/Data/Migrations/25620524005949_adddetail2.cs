using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Data.Migrations
{
    public partial class adddetail2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Detail2Rate1",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Detail2Rate2",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Detail2Rate3",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Detail2Rate4",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Detail2Rate5",
                table: "PointOfEvaluations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Detail2Rate1",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "Detail2Rate2",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "Detail2Rate3",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "Detail2Rate4",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "Detail2Rate5",
                table: "PointOfEvaluations");
        }
    }
}
