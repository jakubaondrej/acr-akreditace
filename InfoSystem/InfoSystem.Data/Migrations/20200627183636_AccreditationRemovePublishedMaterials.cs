using Microsoft.EntityFrameworkCore.Migrations;

namespace InfoSystem.Data.Migrations
{
    public partial class AccreditationRemovePublishedMaterials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishedMaterials",
                table: "Accreditation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PublishedMaterials",
                table: "Accreditation",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
