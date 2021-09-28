using Microsoft.EntityFrameworkCore.Migrations;

namespace InfoSystem.Data.Migrations
{
    public partial class UserAccessRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Redaction_RedactionId",
                table: "User");

            migrationBuilder.AlterColumn<int>(
                name: "RedactionId",
                table: "User",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "UserAccessRequest",
                columns: table => new
                {
                    UserAccessRequestId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Role = table.Column<string>(nullable: true),
                    CallNumber = table.Column<string>(nullable: true),
                    Redaction = table.Column<string>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccessRequest", x => x.UserAccessRequestId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_User_Redaction_RedactionId",
                table: "User",
                column: "RedactionId",
                principalTable: "Redaction",
                principalColumn: "RedactionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Redaction_RedactionId",
                table: "User");

            migrationBuilder.DropTable(
                name: "UserAccessRequest");

            migrationBuilder.AlterColumn<int>(
                name: "RedactionId",
                table: "User",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_User_Redaction_RedactionId",
                table: "User",
                column: "RedactionId",
                principalTable: "Redaction",
                principalColumn: "RedactionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
