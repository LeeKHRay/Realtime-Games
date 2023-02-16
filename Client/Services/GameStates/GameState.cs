using RealtimeGames.Shared.Models;

namespace RealtimeGames.Client.Services.GameStates;

public enum GameResult
{
    NotEnd, Player1Win, Player2Win, Draw
}

public abstract class GameState : IGameState
{
    private const string Blue = "blue";
    private const string Red = "red";

    protected bool isPlayer1;

    public abstract string GameName { get; }
    public string? PlayerColor { get; set; }
    public string? OpponentColor { get; set; }
    public bool IsPlayerTurn { get; set; }
    public Player[] Players { get; } = new Player[2];
    public GameResult GameResult { get; set; }

    public virtual void SetState(Player player1, Player player2, bool isPlayer1)
    {
        PlayerColor = isPlayer1 ? Blue : Red;
        OpponentColor = isPlayer1 ? Red : Blue;
        IsPlayerTurn = isPlayer1;
        Players[0] = player1;
        Players[1] = player2;
        GameResult = GameResult.NotEnd;
        this.isPlayer1 = isPlayer1;
    }

    public bool IsWin() => (isPlayer1 && GameResult == GameResult.Player1Win) || (!isPlayer1 && GameResult == GameResult.Player2Win);
}