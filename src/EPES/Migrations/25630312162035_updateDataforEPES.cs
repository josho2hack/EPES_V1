using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Migrations
{
    public partial class updateDataforEPES : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ResultLevelRate",
                table: "DataForEvaluations",
                type: "decimal(38, 10)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(38, 10)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ResultLevelRate",
                table: "DataForEvaluations",
                type: "decimal(38, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(38, 10)",
                oldDefaultValue: 0m);
        }
    }
}
