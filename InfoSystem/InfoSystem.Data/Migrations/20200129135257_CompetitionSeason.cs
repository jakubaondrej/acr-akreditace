using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InfoSystem.Data.Migrations
{
    public partial class CompetitionSeason : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competition_Championships_ChampionshipId",
                table: "Competition");

            migrationBuilder.DropForeignKey(
                name: "FK_Championships_Sport_SportId",
                table: "Championships");

            migrationBuilder.DropColumn(
                name: "End",
                table: "Competition");

            migrationBuilder.DropColumn(
                name: "JournalistRegistrationEnd",
                table: "Competition");

            migrationBuilder.DropColumn(
                name: "JournalistRegistrationStart",
                table: "Competition");

            migrationBuilder.DropColumn(
                name: "JournalistUploadFotoDeadline",
                table: "Competition");

            migrationBuilder.DropColumn(
                name: "PictureStoreUri",
                table: "Competition");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "Competition");

            migrationBuilder.AlterColumn<int>(
                name: "SportId",
                table: "Championships",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ChampionshipId",
                table: "Competition",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CompetitionSeason",
                columns: table => new
                {
                    CompetitionSeasonId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompetitionId = table.Column<int>(nullable: false),
                    SeasonId = table.Column<int>(nullable: false),
                    PictureStoreUri = table.Column<string>(nullable: true),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    JournalistRegistrationStart = table.Column<DateTime>(nullable: false),
                    JournalistRegistrationEnd = table.Column<DateTime>(nullable: false),
                    JournalistUploadFotoDeadline = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionSeason", x => x.CompetitionSeasonId);
                    table.ForeignKey(
                        name: "FK_CompetitionSeason_Competition_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competition",
                        principalColumn: "CompetitionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetitionSeason_Season_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Season",
                        principalColumn: "SeasonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionSeason_CompetitionId",
                table: "CompetitionSeason",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionSeason_SeasonId",
                table: "CompetitionSeason",
                column: "SeasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Competition_Championships_ChampionshipId",
                table: "Competition",
                column: "ChampionshipId",
                principalTable: "Championships",
                principalColumn: "ChampionshipId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Championships_Sport_SportId",
                table: "Championships",
                column: "SportId",
                principalTable: "Sport",
                principalColumn: "SportId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competition_Championships_ChampionshipId",
                table: "Competition");

            migrationBuilder.DropForeignKey(
                name: "FK_Championships_Sport_SportId",
                table: "Championships");

            migrationBuilder.DropTable(
                name: "CompetitionSeason");

            migrationBuilder.AlterColumn<int>(
                name: "SportId",
                table: "Championships",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ChampionshipId",
                table: "Competition",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<DateTime>(
                name: "End",
                table: "Competition",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "JournalistRegistrationEnd",
                table: "Competition",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "JournalistRegistrationStart",
                table: "Competition",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "JournalistUploadFotoDeadline",
                table: "Competition",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PictureStoreUri",
                table: "Competition",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Start",
                table: "Competition",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Competition_Championships_ChampionshipId",
                table: "Competition",
                column: "ChampionshipId",
                principalTable: "Championships",
                principalColumn: "ChampionshipId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Championships_Sport_SportId",
                table: "Championships",
                column: "SportId",
                principalTable: "Sport",
                principalColumn: "SportId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
