using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BancolombiaStarter.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addpicturetouser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "AspNetUsers");
        }
    }
}
