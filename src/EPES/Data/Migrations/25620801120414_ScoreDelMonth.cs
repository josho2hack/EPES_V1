using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Data.Migrations
{
    public partial class ScoreDelMonth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastMonth",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "LastMonth",
                table: "ScoreDrafts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastMonth",
                table: "Scores",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastMonth",
                table: "ScoreDrafts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
