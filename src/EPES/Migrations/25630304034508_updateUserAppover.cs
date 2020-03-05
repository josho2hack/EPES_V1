using Microsoft.EntityFrameworkCore.Migrations;

namespace EPES.Migrations
{
    public partial class updateUserAppover : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "approver",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "approver",
                table: "AspNetUsers");
        }
    }
}
