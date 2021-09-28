using Microsoft.EntityFrameworkCore.Migrations;

namespace InfoSystem.Data.Migrations
{
    public partial class Accreditation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accreditation",
                columns: table => new
                {
                    AccreditationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    CompetitionSeasonId = table.Column<int>(nullable: false),
                    State = table.Column<string>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    PublishedMaterials = table.Column<bool>(nullable: false),
                    Close = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accreditation", x => x.AccreditationId);
                    table.ForeignKey(
                        name: "FK_Accreditation_CompetitionSeason_CompetitionSeasonId",
                        column: x => x.CompetitionSeasonId,
                        principalTable: "CompetitionSeason",
                        principalColumn: "CompetitionSeasonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accreditation_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accreditation_CompetitionSeasonId",
                table: "Accreditation",
                column: "CompetitionSeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Accreditation_UserId",
                table: "Accreditation",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accreditation");
        }
    }
}
