namespace RealtimeGames.Shared.Models;

public class Score
{
    public int Connect4Score { get; set; } = 0;
    public int BattleshipScore { get; set; } = 0;

    public int this[string gameName]
    {
        get => gameName switch
        {
            "connect4" => Connect4Score,
            "battleship" => BattleshipScore,
            _ => throw new KeyNotFoundException("Invalid game name")
        };

        set
        {
            switch (gameName)
            {
                case "connect4":
                    Connect4Score = value;
                    break;
                case "battleship":
                    BattleshipScore = value;
                    break;
                default:
                    throw new KeyNotFoundException("Invalid game name");
            };
        }
    }
}