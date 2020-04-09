using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Migrations
{
    public partial class updateOfficeaddGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OfficeGroupId",
                table: "Offices",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offices_OfficeGroupId",
                table: "Offices",
                column: "OfficeGroupId",
                unique: true,
                filter: "[OfficeGroupId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Offices_Offices_OfficeGroupId",
                table: "Offices",
                column: "OfficeGroupId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offices_Offices_OfficeGroupId",
                table: "Offices");

            migrationBuilder.DropIndex(
                name: "IX_Offices_OfficeGroupId",
                table: "Offices");

            migrationBuilder.DropColumn(
                name: "OfficeGroupId",
                table: "Offices");
        }
    }
}
