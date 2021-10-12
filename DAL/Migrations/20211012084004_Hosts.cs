using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class Hosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetUsers_ApplicationUserId",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Employees",
                newName: "VisitorId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_ApplicationUserId",
                table: "Employees",
                newName: "IX_Employees_VisitorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AspNetUsers_VisitorId",
                table: "Employees",
                column: "VisitorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetUsers_VisitorId",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "VisitorId",
                table: "Employees",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_VisitorId",
                table: "Employees",
                newName: "IX_Employees_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AspNetUsers_ApplicationUserId",
                table: "Employees",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
