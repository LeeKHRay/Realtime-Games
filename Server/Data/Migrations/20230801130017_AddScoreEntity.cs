using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealtimeGames.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddScoreEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BattleshipScore",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Connect4Score",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Connect4Score = table.Column<int>(type: "int", nullable: false),
                    BattleshipScore = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.ApplicationUserId);
                    table.ForeignKey(
                        name: "FK_Scores_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Scores");

            migrationBuilder.AddColumn<int>(
                name: "BattleShipScore",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Connect4Score",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
