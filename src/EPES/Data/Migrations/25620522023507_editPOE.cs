using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Data.Migrations
{
    public partial class editPOE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DetailPlan",
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

            migrationBuilder.AlterColumn<int>(
                name: "Approve",
                table: "DataForEvaluations",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
