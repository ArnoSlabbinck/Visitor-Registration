using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class Image : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CompanyPhoto",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "VisitorPhoto",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "PictureImageId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PictureImageId",
                table: "Companies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PictureImageId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    OriginalFormat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageFile = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ImageId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PictureImageId",
                table: "Employees",
                column: "PictureImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_PictureImageId",
                table: "Companies",
                column: "PictureImageId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PictureImageId",
                table: "AspNetUsers",
                column: "PictureImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Images_PictureImageId",
                table: "AspNetUsers",
                column: "PictureImageId",
                principalTable: "Images",
                principalColumn: "ImageId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Images_PictureImageId",
                table: "Companies",
                column: "PictureImageId",
                principalTable: "Images",
                principalColumn: "ImageId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Images_PictureImageId",
                table: "Employees",
                column: "PictureImageId",
                principalTable: "Images",
                principalColumn: "ImageId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Images_PictureImageId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Images_PictureImageId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Images_PictureImageId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Employees_PictureImageId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Companies_PictureImageId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PictureImageId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PictureImageId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PictureImageId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "PictureImageId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyPhoto",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VisitorPhoto",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
