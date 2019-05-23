using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Data.Migrations
{
    public partial class updatedata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Unit",
                table: "PointOfEvaluations",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DetailRate1",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DetailRate2",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DetailRate3",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DetailRate4",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DetailRate5",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Result",
                table: "DataForEvaluations",
                type: "decimal(38, 10)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(38, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "OldResult",
                table: "DataForEvaluations",
                type: "decimal(38, 10)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(38, 10)");

            migrationBuilder.AlterColumn<int>(
                name: "Month",
                table: "DataForEvaluations",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<decimal>(
                name: "Expect",
                table: "DataForEvaluations",
                type: "decimal(38, 10)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(38, 10)");

            migrationBuilder.AlterColumn<int>(
                name: "Approve",
                table: "DataForEvaluations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DetailPlan",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "DetailRate1",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "DetailRate2",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "DetailRate3",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "DetailRate4",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "DetailRate5",
                table: "PointOfEvaluations");

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "PointOfEvaluations",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Result",
                table: "DataForEvaluations",
                type: "decimal(38, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38, 10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "OldResult",
                table: "DataForEvaluations",
                type: "decimal(38, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38, 10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Month",
                table: "DataForEvaluations",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<decimal>(
                name: "Expect",
                table: "DataForEvaluations",
                type: "decimal(38, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38, 10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Approve",
                table: "DataForEvaluations",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
