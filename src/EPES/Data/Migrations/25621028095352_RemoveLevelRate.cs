using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Data.Migrations
{
    public partial class RemoveLevelRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LevelRates");

            migrationBuilder.AddColumn<decimal>(
                name: "LRate1",
                table: "PointOfEvaluations",
                type: "decimal(18, 4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LRate2",
                table: "PointOfEvaluations",
                type: "decimal(18, 4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LRate3",
                table: "PointOfEvaluations",
                type: "decimal(18, 4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LRate4",
                table: "PointOfEvaluations",
                type: "decimal(18, 4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LRate5",
                table: "PointOfEvaluations",
                type: "decimal(18, 4)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LRate1",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "LRate2",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "LRate3",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "LRate4",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "LRate5",
                table: "PointOfEvaluations");

            migrationBuilder.CreateTable(
                name: "LevelRates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PointOfEvaluationId = table.Column<int>(nullable: false),
                    Rate1 = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    Rate2 = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    Rate3 = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    Rate4 = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    Rate5 = table.Column<decimal>(type: "decimal(18, 4)", nullable: false)
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
    }
}
