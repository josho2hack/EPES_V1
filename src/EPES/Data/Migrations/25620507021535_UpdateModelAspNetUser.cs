using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Data.Migrations
{
    public partial class UpdateModelAspNetUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataForEvaluations_Offices_OfficeId",
                table: "DataForEvaluations");

            migrationBuilder.DropForeignKey(
                name: "FK_DataForEvaluations_PointOfEvaluations_PointOfEvaluationId",
                table: "DataForEvaluations");

            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Offices_OfficeId",
                table: "Scores");

            migrationBuilder.DropForeignKey(
                name: "FK_Scores_PointOfEvaluations_PointOfEvaluationId",
                table: "Scores");

            migrationBuilder.RenameColumn(
                name: "PointOfEvaluationId",
                table: "Scores",
                newName: "PointOfEvaluationID");

            migrationBuilder.RenameColumn(
                name: "OfficeId",
                table: "Scores",
                newName: "OfficeID");

            migrationBuilder.RenameIndex(
                name: "IX_Scores_PointOfEvaluationId",
                table: "Scores",
                newName: "IX_Scores_PointOfEvaluationID");

            migrationBuilder.RenameIndex(
                name: "IX_Scores_OfficeId",
                table: "Scores",
                newName: "IX_Scores_OfficeID");

            migrationBuilder.AlterColumn<int>(
                name: "PointOfEvaluationID",
                table: "Scores",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OfficeID",
                table: "Scores",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "PointOfEvaluations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Offices",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PointOfEvaluationId",
                table: "DataForEvaluations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OfficeId",
                table: "DataForEvaluations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "DataForEvaluations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Offices_ApplicationUserId",
                table: "Offices",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DataForEvaluations_Offices_OfficeId",
                table: "DataForEvaluations",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DataForEvaluations_PointOfEvaluations_PointOfEvaluationId",
                table: "DataForEvaluations",
                column: "PointOfEvaluationId",
                principalTable: "PointOfEvaluations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_AspNetUsers_ApplicationUserId",
                table: "Offices",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Offices_OfficeID",
                table: "Scores",
                column: "OfficeID",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_PointOfEvaluations_PointOfEvaluationID",
                table: "Scores",
                column: "PointOfEvaluationID",
                principalTable: "PointOfEvaluations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataForEvaluations_Offices_OfficeId",
                table: "DataForEvaluations");

            migrationBuilder.DropForeignKey(
                name: "FK_DataForEvaluations_PointOfEvaluations_PointOfEvaluationId",
                table: "DataForEvaluations");

            migrationBuilder.DropForeignKey(
                name: "FK_Offices_AspNetUsers_ApplicationUserId",
                table: "Offices");

            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Offices_OfficeID",
                table: "Scores");

            migrationBuilder.DropForeignKey(
                name: "FK_Scores_PointOfEvaluations_PointOfEvaluationID",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Offices_ApplicationUserId",
                table: "Offices");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "PointOfEvaluations");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Offices");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "DataForEvaluations");

            migrationBuilder.RenameColumn(
                name: "PointOfEvaluationID",
                table: "Scores",
                newName: "PointOfEvaluationId");

            migrationBuilder.RenameColumn(
                name: "OfficeID",
                table: "Scores",
                newName: "OfficeId");

            migrationBuilder.RenameIndex(
                name: "IX_Scores_PointOfEvaluationID",
                table: "Scores",
                newName: "IX_Scores_PointOfEvaluationId");

            migrationBuilder.RenameIndex(
                name: "IX_Scores_OfficeID",
                table: "Scores",
                newName: "IX_Scores_OfficeId");

            migrationBuilder.AlterColumn<int>(
                name: "PointOfEvaluationId",
                table: "Scores",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "OfficeId",
                table: "Scores",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "PointOfEvaluationId",
                table: "DataForEvaluations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "OfficeId",
                table: "DataForEvaluations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_DataForEvaluations_Offices_OfficeId",
                table: "DataForEvaluations",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DataForEvaluations_PointOfEvaluations_PointOfEvaluationId",
                table: "DataForEvaluations",
                column: "PointOfEvaluationId",
                principalTable: "PointOfEvaluations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Offices_OfficeId",
                table: "Scores",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_PointOfEvaluations_PointOfEvaluationId",
                table: "Scores",
                column: "PointOfEvaluationId",
                principalTable: "PointOfEvaluations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
