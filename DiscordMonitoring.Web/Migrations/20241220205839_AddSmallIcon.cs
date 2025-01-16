using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordMonitoring.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddSmallIcon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconUrl",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconUrl",
                table: "Games");
        }
    }
}
