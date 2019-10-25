using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Data.Migrations
{
    public partial class LevelRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SubPoint",
                table: "PointOfEvaluations",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "LevelRates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Rate1 = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    Rate2 = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    Rate3 = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    Rate4 = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    Rate5 = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    PointOfEvaluationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LevelRates_PointOfEvaluations_PointOfEvaluationId",
                        column: x => x.PointOfEvaluationId,
                        principalTable: "PointOfEvaluations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LevelRates_PointOfEvaluationId",
                table: "LevelRates",
                column: "PointOfEvaluationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LevelRates");

            migrationBuilder.AlterColumn<int>(
                name: "SubPoint",
                table: "PointOfEvaluations",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);
        }
    }
}
