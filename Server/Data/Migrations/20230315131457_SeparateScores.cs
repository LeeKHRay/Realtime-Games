using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealtimeGames.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeparateScores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Score",
                table: "AspNetUsers",
                newName: "Connect4Score");

            migrationBuilder.AddColumn<int>(
                name: "BattleshipScore",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BattleshipScore",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Connect4Score",
                table: "AspNetUsers",
                newName: "Score");
        }
    }
}
