using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Data.Migrations
{
    public partial class AddMonth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Plan",
                table: "PointOfEvaluations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Rate1MonthStart",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Rate1MonthStart2",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Rate1MonthStop",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Rate1MonthStop2",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Rate2MonthStart",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Rate2MonthStart2",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Rate2MonthStop",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Rate2MonthStop2",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Rate3MonthStart",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Rate3MonthStart2",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Rate3MonthStop",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Rate3MonthStop2",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Rate4MonthStart",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Rate4MonthStart2",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Rate4MonthStop",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Rate4MonthStop2",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Rate5MonthStart",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Rate5MonthStart2",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Rate5MonthStop",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Rate5MonthStop2",
                table: "PointOfEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttachFile",
                table: "DataForEvaluations",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "DataForEvaluations",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rate1MonthStart",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "Rate1MonthStart2",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "Rate1MonthStop",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "Rate1MonthStop2",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "Rate2MonthStart",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "Rate2MonthStart2",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "Rate2MonthStop",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "Rate2MonthStop2",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "Rate3MonthStart",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "Rate3MonthStart2",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "Rate3MonthStop",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "Rate3MonthStop2",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "Rate4MonthStart",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "Rate4MonthStart2",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "Rate4MonthStop",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "Rate4MonthStop2",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "Rate5MonthStart",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "Rate5MonthStart2",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "Rate5MonthStop",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "Rate5MonthStop2",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "AttachFile",
                table: "DataForEvaluations");

            migrationBuilder.DropColumn(
                name: "Completed",
                table: "DataForEvaluations");

            migrationBuilder.AlterColumn<int>(
                name: "Plan",
                table: "PointOfEvaluations",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
