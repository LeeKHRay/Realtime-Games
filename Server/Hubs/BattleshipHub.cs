using Microsoft.AspNetCore.SignalR;
using RealtimeGames.Server.Services;

namespace RealtimeGames.Server.Hubs;

public class BattleshipHub : GameHub
{
    public BattleshipHub(GameHubPlayersStorage gameHubPlayersStorage) : base(gameHubPlayersStorage.GameHubPlayers["battleship"]) { }

    public async Task AttackShip(int row, int col, string playerName)
    {
        await Clients.Group(playerName).SendAsync("AttackShip", row, col);
    }

    public async Task SendAttackResult(string updatedCells, string playerName)
    {
        await Clients.Group(playerName).SendAsync("ReceiveAttackResult", updatedCells);
    }
}
