using Microsoft.EntityFrameworkCore.Migrations;

namespace InfoSystem.Data.Migrations
{
    public partial class RedactorType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RedactorType",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RedactorType",
                table: "AspNetUsers");
        }
    }
}
