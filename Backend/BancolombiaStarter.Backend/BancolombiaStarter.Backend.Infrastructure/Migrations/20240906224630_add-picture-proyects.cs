using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BancolombiaStarter.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addpictureproyects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "Projects");
        }
    }
}
