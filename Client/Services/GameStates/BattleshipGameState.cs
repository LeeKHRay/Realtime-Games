using RealtimeGames.Client.Models;
using RealtimeGames.Shared.Models;
using CellState = RealtimeGames.Client.Models.BattleshipCell.CellState;

namespace RealtimeGames.Client.Services.GameStates;

public class BattleshipGameState : GameState, IBattleshipGameState
{
    private readonly HashSet<(int, int)>[] _shipsPos = new HashSet<(int, int)>[5];
    private int[] playerShipCellNum = default!;
    private int opponentShipNum;

    public override string GameName => "battleship";

    public BattleshipCell[,] PlayerGrid { get; } = new BattleshipCell[10, 10];
    public BattleshipCell[,] OpponentGrid { get; } = new BattleshipCell[10, 10];
    public bool CanClick { get; set; }

    public override void SetState(Player player1, Player player2, bool isPlayer1)
    {
        base.SetState(player1, player2, isPlayer1);

        for (int r = 0; r < 10; r++)
        {
            for (int c = 0; c < 10; c++)
            {
                PlayerGrid[r, c] = new BattleshipCell(r, c, CellState.Empty);
                OpponentGrid[r, c] = new BattleshipCell(r, c, CellState.Attackable);
            }
        }

        CanClick = true;
        playerShipCellNum = new int[5] { 2, 3, 4, 4, 5 };
        opponentShipNum = 5;
        for (int i = 0; i < _shipsPos.Length; i++)
        {
            _shipsPos[i] = new HashSet<(int, int)>(playerShipCellNum[i]);
        }

        PlaceShips();
    }

    public void PrintBoard()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Console.Write(PlayerGrid[i, j].State.ToString());
            }
            Console.WriteLine();
        }
    }

    private void PlaceShips()
    {
        HashSet<(int, int, int, int)> shipRange = new(5);
        Random random = new();

        for (int i = 0; i < playerShipCellNum.Length; i++)
        {
            while (true)
            {
                int startR = random.Next(10);
                int endR = startR;
                int startC = random.Next(10);
                int endC = startC;

                if (random.NextDouble() < 0.5) // horizontal
                {
                    endC += playerShipCellNum[i] - 1;
                    if (endC >= 10)
                    {
                        continue;
                    }
                }
                else // vertical
                {
                    endR += playerShipCellNum[i] - 1;
                    if (endR >= 10)
                    {
                        continue;
                    }
                }

                // check if the new ship intersect with other added ships
                bool canPlaceShip = true;
                foreach ((int shipStartR, int shipStartC, int shipEndR, int shipEndC) in shipRange)
                {
                    if (startC <= shipEndC && shipStartC <= endC && startR <= shipEndR && shipStartR <= endR)
                    {
                        canPlaceShip = false;
                        break;
                    }
                }

                // Place the new ship on the grid
                if (canPlaceShip)
                {
                    CellState shipType = (CellState)(1 << i);
                    for (int r = startR; r <= endR; r++)
                    {
                        for (int c = startC; c <= endC; c++)
                        {
                            PlayerGrid[r, c].State = shipType;
                            _shipsPos[i].Add((r, c));
                        }
                    }
                    shipRange.Add((startR, startC, endR, endC));
                    break;
                }
            }
        }
    }

    public bool CanAttackCell(int row, int col)
    {
        return OpponentGrid[row, col].IsAttackable;
    }

    // opponent damage player's ship
    public List<BattleshipCell?> DamageShip(int row, int col)
    {
        List<BattleshipCell?> updatedCells = new();

        int shipId = PlayerGrid[row, col].FindShipId();
        if (shipId != -1) // check if the cell is occupied by a ship
        {
            playerShipCellNum[shipId - 1]--;
            if (playerShipCellNum[shipId - 1] == 0) // the ship is destroyed
            {
                foreach ((int r, int c) in _shipsPos[shipId - 1])
                {
                    PlayerGrid[r, c].State |= CellState.Destroyed;
                    updatedCells.Add(PlayerGrid[r, c]);
                }
                CheckWin();

                if (playerShipCellNum.Sum() == 0) // all ships are destroyed
                {
                    IsPlayerTurn = !IsPlayerTurn;
                    updatedCells.Add(null); // notify opponent to negate IsPlayerTurn
                }
            }
            else
            {
                PlayerGrid[row, col].State |= CellState.Damaged;
                updatedCells.Add(PlayerGrid[row, col]);
            }
        }
        else
        {
            CheckWin();
            IsPlayerTurn = !IsPlayerTurn;
            updatedCells.Add(PlayerGrid[row, col]);
        }

        return updatedCells;
    }

    public void UpdateOpponentBoard(List<BattleshipCell> updatedCells)
    {
        CellState state = default;
        foreach (BattleshipCell cell in updatedCells)
        {
            if (cell != null)
            {
                if (cell.IsShip)
                {
                    if (cell.IsDestroyed)
                    {
                        OpponentGrid[cell.Row, cell.Col] = cell;
                    }
                    else
                    {
                        state = CellState.Damaged;
                    }
                }
                else
                {
                    state = CellState.NotAttackable;
                }
            }
        }

        if (updatedCells.Count > 1) // ship is destroyed
        {
            opponentShipNum--;
            CheckWin();

            if (updatedCells.Any(cell => cell == null)) // ship is destroyed
            {
                IsPlayerTurn = !IsPlayerTurn;
            }
        }
        else
        {
            OpponentGrid[updatedCells[0].Row, updatedCells[0].Col].State = state;
            if (state == CellState.NotAttackable)
            {
                CheckWin();
                IsPlayerTurn = !IsPlayerTurn;
            }
        }

        CanClick = true;
    }

    private void CheckWin()
    {
        GameResult playerWin = isPlayer1 ? GameResult.Player1Win : GameResult.Player2Win;
        GameResult opponentWin = isPlayer1 ? GameResult.Player2Win : GameResult.Player1Win;
        int playerShipNum = playerShipCellNum.Count(n => n > 0);

        Console.WriteLine($"CheckWin {playerShipNum} {opponentShipNum} {GameResult}");
        if (opponentShipNum == 0 && playerShipNum == 0)
        {
            GameResult = GameResult.Draw;
        }
        else if (opponentShipNum == 0)
        {
            // if player 2 destroy all player 1's ships, player 2 must win
            // because the current round is ended
            if (!isPlayer1 || playerShipNum > 1 || !IsPlayerTurn)
            {
                GameResult = playerWin;
            }
        }
        else if (playerShipNum == 0)
        {
            // if all player 1's ships are destroyed, player 1 must lose
            // because the current round is ended and player 1 has no chance to destroy player 2's ship even enough player 2 only has 1 ship left
            // if it is player 2's turn and player 2 and cannot destroy all player 1's ships
            if (isPlayer1 || opponentShipNum > 1 || IsPlayerTurn)
            {
                GameResult = opponentWin;
            }
        }
    }
}
