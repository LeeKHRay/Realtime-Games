namespace RealtimeGames.Client.Models;

public class BattleshipCell : Cell<BattleshipCell.CellState>
{
    public enum CellState
    {
        Empty = 0, Ship1 = 1, Ship2 = 1 << 1, Ship3 = 1 << 2, Ship4 = 1 << 3, Ship5 = 1 << 4, Ship = Ship1 | Ship2 | Ship3 | Ship4 | Ship5,
        Damaged = 1 << 5, Destroyed = 1 << 6, Attackable = 1 << 7, NotAttackable = 1 << 8,
    }

    public BattleshipCell(int row, int col, CellState state) : base(row, col, state) { }

    public string ShipType
    {
        get
        {
            int shipId = FindShipId();
            if (shipId != -1)
            {
                return $"ship{shipId}";
            }

            return "";
        }
    }

    public bool IsShip => (State & CellState.Ship) > 0;
    public bool IsDamaged => (State & CellState.Damaged) == CellState.Damaged;
    public bool IsDestroyed => (State & CellState.Destroyed) == CellState.Destroyed;
    public bool IsAttackable => State == CellState.Attackable;

    public int FindShipId()
    {
        if (IsShip)
        {
            for (int i = 0; i < 5; i++)
            {
                CellState ship = (CellState)(1 << i);
                if ((State & ship) == ship)
                {
                    return i + 1;
                }
            }
        }

        return -1;
    }
}
