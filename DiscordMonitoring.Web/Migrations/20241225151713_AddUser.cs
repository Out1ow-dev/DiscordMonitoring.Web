using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordMonitoring.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Servers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Servers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Servers_UserId1",
                table: "Servers",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Servers_AspNetUsers_UserId1",
                table: "Servers",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servers_AspNetUsers_UserId1",
                table: "Servers");

            migrationBuilder.DropIndex(
                name: "IX_Servers_UserId1",
                table: "Servers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Servers");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Servers");
        }
    }
}
