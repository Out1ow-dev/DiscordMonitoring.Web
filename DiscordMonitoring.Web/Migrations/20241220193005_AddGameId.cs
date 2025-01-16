using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordMonitoring.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddGameId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "GameSteamId",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameSteamId",
                table: "Games");
        }
    }
}
