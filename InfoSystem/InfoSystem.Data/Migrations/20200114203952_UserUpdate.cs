using Microsoft.EntityFrameworkCore.Migrations;

namespace InfoSystem.Data.Migrations
{
    public partial class UserUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RedactionId",
                table: "User",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_RedactionId",
                table: "User",
                column: "RedactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Redaction_RedactionId",
                table: "User",
                column: "RedactionId",
                principalTable: "Redaction",
                principalColumn: "RedactionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Redaction_RedactionId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_RedactionId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "RedactionId",
                table: "User");
        }
    }
}
