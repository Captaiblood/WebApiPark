using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkiAPI.Migrations
{
    public partial class AddParkPictureColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ParkPicture",
                table: "NationalParks",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParkPicture",
                table: "NationalParks");
        }
    }
}
