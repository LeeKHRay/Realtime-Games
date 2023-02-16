using RealtimeGames.Shared.Models;

namespace RealtimeGames.Client.Services.GameStates;

public interface IGameState
{
    string GameName { get; }
    string? PlayerColor { get; set; }
    string? OpponentColor { get; set; }
    bool IsPlayerTurn { get; set; }
    Player[] Players { get; }
    GameResult GameResult { get; set; }

    void SetState(Player player1, Player player2, bool isPlayer1);
    bool IsWin();
}