using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Migrations
{
    public partial class nullAbleApprove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Approve",
                table: "DataForEvaluations",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Approve",
                table: "DataForEvaluations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
