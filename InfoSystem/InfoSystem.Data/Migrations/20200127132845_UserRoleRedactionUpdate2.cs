using Microsoft.EntityFrameworkCore.Migrations;

namespace InfoSystem.Data.Migrations
{
    public partial class UserRoleRedactionUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Redaction_RedactionId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "RedactionId",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Redaction_RedactionId",
                table: "AspNetUsers",
                column: "RedactionId",
                principalTable: "Redaction",
                principalColumn: "RedactionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Redaction_RedactionId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "RedactionId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Redaction_RedactionId",
                table: "AspNetUsers",
                column: "RedactionId",
                principalTable: "Redaction",
                principalColumn: "RedactionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
