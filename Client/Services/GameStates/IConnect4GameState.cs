using RealtimeGames.Client.Models;

namespace RealtimeGames.Client.Services.GameStates;

public interface IConnect4GameState : IGameState
{
    Connect4Cell[,] Grid { get; }

    int FindLowestEmptyCell(int col);
    void PutChecker(int row, int col);
}