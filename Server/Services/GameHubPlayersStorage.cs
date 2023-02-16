using RealtimeGames.Server.Models;

namespace RealtimeGames.Server.Services;

public class GameHubPlayersStorage
{
    public Dictionary<string, GameHubPlayers> GameHubPlayers { get; } = new()
    {
        { "connect4", new() },
        { "battleship", new() }
    };
}
