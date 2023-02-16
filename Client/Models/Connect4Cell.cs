namespace RealtimeGames.Client.Models;

public class Connect4Cell : Cell<Connect4Cell.CellState>
{
    public enum CellState
    {
        Empty, Player1, Player2
    }

    public Connect4Cell(int row, int col) : base(row, col, CellState.Empty) { }

    public string Color {
        get {
            if (State == CellState.Player1)
            {
                return "blue";
            }
            if (State == CellState.Player2)
            {
                return "red";
            }
            return "gray";
        }
    }
}
