using Microsoft.EntityFrameworkCore.Migrations;

namespace InfoSystem.Data.Migrations
{
    public partial class CompetitionImageUri : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUri",
                table: "Competition",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUri",
                table: "Competition");
        }
    }
}
