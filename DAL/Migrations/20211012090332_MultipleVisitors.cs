using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class MultipleVisitors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetUsers_VisitorId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_VisitorId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "VisitorId",
                table: "Employees");

            migrationBuilder.CreateTable(
                name: "ApplicationUserEmployee",
                columns: table => new
                {
                    HostsId = table.Column<int>(type: "int", nullable: false),
                    VisitorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserEmployee", x => new { x.HostsId, x.VisitorId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserEmployee_AspNetUsers_VisitorId",
                        column: x => x.VisitorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserEmployee_Employees_HostsId",
                        column: x => x.HostsId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserEmployee_VisitorId",
                table: "ApplicationUserEmployee",
                column: "VisitorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserEmployee");

            migrationBuilder.AddColumn<string>(
                name: "VisitorId",
                table: "Employees",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_VisitorId",
                table: "Employees",
                column: "VisitorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AspNetUsers_VisitorId",
                table: "Employees",
                column: "VisitorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
