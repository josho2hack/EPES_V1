using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Migrations
{
    public partial class addFixAndCalpermonthPOEwithNotNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "FixExpect",
                table: "PointOfEvaluations",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "CalPerMonth",
                table: "PointOfEvaluations",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "FixExpect",
                table: "PointOfEvaluations",
                nullable: true,
                oldClrType: typeof(bool),
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "CalPerMonth",
                table: "PointOfEvaluations",
                nullable: true,
                oldClrType: typeof(bool),
                oldDefaultValue: false);
        }
    }
}
