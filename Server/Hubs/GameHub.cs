using Microsoft.AspNetCore.SignalR;
using RealtimeGames.Server.Models;

namespace RealtimeGames.Server.Hubs;

public abstract class GameHub : Hub
{
    private readonly GameHubPlayers _gameHubPlayers;

    public GameHub(GameHubPlayers gameHubPlayers)
    {
        _gameHubPlayers = gameHubPlayers;
    }

    public override async Task OnConnectedAsync()
    {
        string playerName = Context.GetHttpContext()!.Request.Query["playerName"]!;

        _gameHubPlayers.AddPlayer(playerName, Context.ConnectionId);
        _gameHubPlayers.ShowUsers();

        await Groups.AddToGroupAsync(Context.ConnectionId, playerName);

        await base.OnConnectedAsync();
    }

    public async Task WaitPlayer(string playerName)
    {
        List<string> names = _gameHubPlayers.GetAllWaitingPlayerNames();
        if (names.Count > 0)
        {
            int idx = new Random().Next(0, names.Count); // randomly match with a player
            _gameHubPlayers.CancelWaiting(names[idx]);
            await JoinGame(playerName, names[idx]);
        }
        else
        {
            _gameHubPlayers.WaitPlayer(playerName);
            await Clients.Group(playerName).SendAsync("WaitPlayer");
        }
    }

    public async Task CancelWaiting(string playerName)
    {
        _gameHubPlayers.CancelWaiting(playerName);
        await Clients.Group(playerName).SendAsync("CancelWaiting");
    }

    public async Task JoinGame(string player1Name, string player2Name)
    {
        string roomId = Guid.NewGuid().ToString(); // generate unique roomId
        await Groups.AddToGroupAsync(_gameHubPlayers.GetId(player1Name), roomId);
        await Groups.AddToGroupAsync(_gameHubPlayers.GetId(player2Name), roomId);

        // randomly choose the first player
        if (new Random().NextDouble() < 0.5)
        {
            (player1Name, player2Name) = (player2Name, player1Name);
        }

        await Clients.Group(roomId).SendAsync("StartGame", player1Name, player2Name, roomId);
    }

    public async Task QuitGame(string playerName, string roomId)
    {
        _gameHubPlayers.CancelWaiting(playerName);
        await Clients.Group(playerName).SendAsync("QuitGame");
        await Groups.RemoveFromGroupAsync(_gameHubPlayers.GetId(playerName), roomId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _gameHubPlayers.TryRemovePlayer(Context.ConnectionId);

        await base.OnDisconnectedAsync(exception);
    }
}
