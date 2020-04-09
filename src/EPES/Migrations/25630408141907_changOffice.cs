using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Migrations
{
    public partial class changOffice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Offices_OfficeGroupId",
                table: "Offices");

            migrationBuilder.CreateIndex(
                name: "IX_Offices_OfficeGroupId",
                table: "Offices",
                column: "OfficeGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Offices_OfficeGroupId",
                table: "Offices");

            migrationBuilder.CreateIndex(
                name: "IX_Offices_OfficeGroupId",
                table: "Offices",
                column: "OfficeGroupId",
                unique: true,
                filter: "[OfficeGroupId] IS NOT NULL");
        }
    }
}
