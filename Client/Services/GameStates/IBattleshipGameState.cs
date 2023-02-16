using RealtimeGames.Client.Models;

namespace RealtimeGames.Client.Services.GameStates;

public interface IBattleshipGameState : IGameState
{
    BattleshipCell[,] PlayerGrid { get; }
    BattleshipCell[,] OpponentGrid { get; }
    bool CanClick { get; set; }

    bool CanAttackCell(int row, int col);
    List<BattleshipCell?> DamageShip(int row, int col);
    void UpdateOpponentBoard(List<BattleshipCell> updatedCells);
}