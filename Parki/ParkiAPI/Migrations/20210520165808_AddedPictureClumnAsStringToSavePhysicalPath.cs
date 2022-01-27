using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkiAPI.Migrations
{
    public partial class AddedPictureClumnAsStringToSavePhysicalPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "NationalParks",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "NationalParks");
        }
    }
}
