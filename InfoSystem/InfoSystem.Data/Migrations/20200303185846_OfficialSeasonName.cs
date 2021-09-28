using Microsoft.EntityFrameworkCore.Migrations;

namespace InfoSystem.Data.Migrations
{
    public partial class OfficialSeasonName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DriveFolderId",
                table: "CompetitionSeason",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfficialSeasonName",
                table: "CompetitionSeason",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RedactorReport",
                columns: table => new
                {
                    RedactorReportId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriveFileId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    CompetitionSeasonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedactorReport", x => x.RedactorReportId);
                    table.ForeignKey(
                        name: "FK_RedactorReport_CompetitionSeason_CompetitionSeasonId",
                        column: x => x.CompetitionSeasonId,
                        principalTable: "CompetitionSeason",
                        principalColumn: "CompetitionSeasonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RedactorReport_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RedactorViewPaparaziMedia",
                columns: table => new
                {
                    RedactorViewPaparaziMediaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    CompetitionSeasonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedactorViewPaparaziMedia", x => x.RedactorViewPaparaziMediaId);
                    table.ForeignKey(
                        name: "FK_RedactorViewPaparaziMedia_CompetitionSeason_CompetitionSeasonId",
                        column: x => x.CompetitionSeasonId,
                        principalTable: "CompetitionSeason",
                        principalColumn: "CompetitionSeasonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RedactorViewPaparaziMedia_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RedactorReport_CompetitionSeasonId",
                table: "RedactorReport",
                column: "CompetitionSeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_RedactorReport_UserId",
                table: "RedactorReport",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RedactorViewPaparaziMedia_CompetitionSeasonId",
                table: "RedactorViewPaparaziMedia",
                column: "CompetitionSeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_RedactorViewPaparaziMedia_UserId",
                table: "RedactorViewPaparaziMedia",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RedactorReport");

            migrationBuilder.DropTable(
                name: "RedactorViewPaparaziMedia");

            migrationBuilder.DropColumn(
                name: "DriveFolderId",
                table: "CompetitionSeason");

            migrationBuilder.DropColumn(
                name: "OfficialSeasonName",
                table: "CompetitionSeason");
        }
    }
}
