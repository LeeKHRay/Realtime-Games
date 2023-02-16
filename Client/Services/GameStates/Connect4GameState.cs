using RealtimeGames.Client.Models;
using RealtimeGames.Shared.Models;
using CellState = RealtimeGames.Client.Models.Connect4Cell.CellState;

namespace RealtimeGames.Client.Services.GameStates;

public class Connect4GameState : GameState, IConnect4GameState
{
    private int turn = 1;

    public override string GameName => "connect4";

    public Connect4Cell[,] Grid { get; } = new Connect4Cell[6, 7];

    public override void SetState(Player player1, Player player2, bool isPlayer1)
    {
        base.SetState(player1, player2, isPlayer1);

        for (int r = 0; r < 6; r++)
        {
            for (int c = 0; c < 7; c++)
            {
                Grid[r, c] = new Connect4Cell(r, c);
            }
        }

        turn = 1;
    }

    public void PrintBoard()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                Console.Write(Grid[i, j].State.ToString());
            }
            Console.WriteLine();
        }
    }

    public int FindLowestEmptyCell(int col)
    {
        int row = 5;
        while (row >= 0)
        {
            if (Grid[row, col].State == CellState.Empty) // find the lowest empty cell in column col
            {
                break;
            }
            row--;
        }

        return row; // return -1 if the column is full
    }

    public void PutChecker(int row, int col)
    {
        Grid[row, col].State = turn % 2 == 1 ? CellState.Player1 : CellState.Player2;
        IsPlayerTurn = !IsPlayerTurn;
        CheckWin(row, col);
        turn++;
    }

    private void CheckWin(int row, int col)
    {
        CellState cellState;
        GameResult result;

        if (turn % 2 == 1)
        {
            cellState = CellState.Player1;
            result = GameResult.Player1Win;
        }
        else
        {
            cellState = CellState.Player2;
            result = GameResult.Player2Win;
        }


        // horizontal
        int startCol = Math.Max(0, col - 3);
        int endCol = Math.Min(col + 3, 6);
        for (int i = startCol, checkerNum = 0; i <= endCol; i++)
        {
            if (Grid[row, i].State == cellState)
            {
                checkerNum++;
                if (checkerNum >= 4)
                {
                    GameResult = result;
                    return;
                }
            }
            else
            {
                checkerNum = 0;
            }
        }

        // vertical
        int startRow = Math.Max(0, row - 3);
        int endRow = Math.Min(row + 3, 5);
        for (int i = startRow, checkerNum = 0; i <= endRow; i++)
        {
            if (Grid[i, col].State == cellState)
            {
                checkerNum++;
                if (checkerNum >= 4)
                {
                    GameResult = result;
                    return;
                }
            }
            else
            {
                checkerNum = 0;
            }
        }

        // diagonal \
        int offset = Math.Min(row - startRow, col - startCol);
        int length = offset + Math.Min(endRow - row, endCol - col) + 1;
        int diagonalStartRow = row - offset;
        int diagonalStartCol = col - offset;
        for (int i = 0, checkerNum = 0; i < length; i++)
        {
            if (Grid[diagonalStartRow + i, diagonalStartCol + i].State == cellState)
            {
                checkerNum++;
                if (checkerNum >= 4)
                {
                    GameResult = result;
                    return;
                }
            }
            else
            {
                checkerNum = 0;
            }
        }

        // diagonal /
        offset = Math.Min(endRow - row, col - startCol);
        length = offset + Math.Min(row - startRow, endCol - col) + 1;
        diagonalStartRow = row + offset;
        diagonalStartCol = col - offset;
        for (int i = 0, checkerNum = 0; i < length; i++)
        {
            if (Grid[diagonalStartRow - i, diagonalStartCol + i].State == cellState)
            {
                checkerNum++;
                if (checkerNum >= 4)
                {
                    GameResult = result;
                    return;
                }
            }
            else
            {
                checkerNum = 0;
            }
        }

        if (turn >= 6 * 7)
        {
            GameResult = GameResult.Draw;
        }
    }
}
