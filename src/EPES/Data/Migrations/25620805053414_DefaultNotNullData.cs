using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Data.Migrations
{
    public partial class DefaultNotNullData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Result",
                table: "DataForEvaluations",
                type: "decimal(38, 10)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(38, 10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Expect",
                table: "DataForEvaluations",
                type: "decimal(38, 10)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(38, 10)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Result",
                table: "DataForEvaluations",
                type: "decimal(38, 10)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(38, 10)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Expect",
                table: "DataForEvaluations",
                type: "decimal(38, 10)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(38, 10)",
                oldDefaultValue: 0m);
        }
    }
}
