using Microsoft.AspNetCore.SignalR;
using RealtimeGames.Server.Services;

namespace RealtimeGames.Server.Hubs;

public class Connect4Hub : GameHub
{
    public Connect4Hub(GameHubPlayersStorage gameHubPlayersStorage) : base(gameHubPlayersStorage.GameHubPlayers["connect4"]) { }

    public async Task PutChecker(int row, int col, string roomId)
    {
        await Clients.Group(roomId).SendAsync("PutChecker", row, col);
    }
}
