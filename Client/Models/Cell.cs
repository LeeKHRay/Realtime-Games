namespace RealtimeGames.Client.Models;

public abstract class Cell<TEnum> where TEnum : Enum
{
    public int Row { get; }
    public int Col { get; }
    public TEnum State { get; set; }

    public Cell(int row, int col, TEnum state = default!)
    {
        Row = row;
        Col = col;
        State = state;
    }
}
