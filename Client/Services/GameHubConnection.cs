using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using RealtimeGames.Client.Services.GameStates;
using RealtimeGames.Shared.Models;
using System.Net.Http.Json;

namespace RealtimeGames.Client.Services;

public class GameHubConnection<T> : IGameHubConnection<T> where T : IGameState
{
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigation;
    private readonly IPlayerState _playerState;
    private readonly IGameState _gameState;
    private HubConnection? hubConnection;

    public bool IsConnected => hubConnection != null && hubConnection.State == HubConnectionState.Connected;

    public GameHubConnection(HttpClient httpClient, NavigationManager navigation, IPlayerState playerState, T gameState)
    {
        _httpClient = httpClient;
        _navigation = navigation;
        _playerState = playerState;
        _gameState = gameState;
    }
    public void CreateConnection(string gameName, string playerName, Action StateHasChanged)
    {
        hubConnection = new HubConnectionBuilder().WithUrl(_navigation.ToAbsoluteUri($"/hubs/{gameName}?playerName={playerName}")).Build(); ;

        hubConnection.On("WaitPlayer", () =>
        {
            _playerState.WaitPlayer();
            StateHasChanged();
        });

        hubConnection.On("CancelWaiting", () =>
        {
            _playerState.CancelWaiting();
            StateHasChanged();
        });

        hubConnection.On<string, string, string>("StartGame", async (player1Name, player2Name, roomId) =>
        {
            bool isPlayer1 = _playerState.Player.Name == player1Name;

            Player player1, player2;
            if (isPlayer1)
            {
                player1 = _playerState.Player;
                player2 = await _httpClient.GetFromJsonAsync<Player>($"{_navigation.BaseUri}api/users/{player2Name}");
            }
            else
            {
                player1 = await _httpClient.GetFromJsonAsync<Player>($"{_navigation.BaseUri}api/users/{player1Name}");
                player2 = _playerState.Player;
            }

            _playerState.StartGame(isPlayer1 ? player2! : player1!, isPlayer1, roomId);
            _gameState.SetState(player1!, player2!, isPlayer1);

            StateHasChanged();
        });

        hubConnection.On("QuitGame", () =>
        {
            _playerState.QuitGame();
            StateHasChanged();
        });
    }

    public Task StartAsync()
    {
        if (hubConnection != null)
        {
            return hubConnection.StartAsync();
        }

        return Task.CompletedTask;
    }

    public async Task WaitPlayer()
    {
        if (IsConnected)
        {
            await hubConnection!.SendAsync("WaitPlayer", _playerState.Player.Name);
        }
    }

    public async Task CancelWaiting()
    {
        if (IsConnected)
        {
            await hubConnection!.SendAsync("CancelWaiting", _playerState.Player.Name);
        }
    }

    public async Task QuitGame()
    {
        if (IsConnected)
        {
            await hubConnection!.SendAsync("QuitGame", _playerState.Player.Name, _playerState.RoomId);
        }
    }

    public ValueTask CloseConnection()
    {
        if (hubConnection != null)
        {
            return hubConnection.DisposeAsync();
        }
        return ValueTask.CompletedTask;
    }

    public void On(string methodName, Action handler)
    {
        if (hubConnection != null)
        {
            hubConnection!.On(methodName, handler);
        }
    }

    public void On<T1>(string methodName, Action<T1> handler)
    {
        if (hubConnection != null)
        {
            hubConnection!.On(methodName, handler);
        }
    }

    public void On<T1, T2>(string methodName, Action<T1, T2> handler)
    {
        if (hubConnection != null)
        {
            hubConnection!.On(methodName, handler);
        }
    }

    public async Task SendAsync(string methodName, object? arg1)
    {
        if (IsConnected)
        {
            await hubConnection!.SendAsync(methodName, arg1);
        }
    }

    public async Task SendAsync(string methodName, object? arg1, object? arg2)
    {
        if (IsConnected)
        {
            await hubConnection!.SendAsync(methodName, arg1, arg2);
        }
    }

    public async Task SendAsync(string methodName, object? arg1, object? arg2, object? arg3)
    {
        if (IsConnected)
        {
            await hubConnection!.SendAsync(methodName, arg1, arg2, arg3);
        }
    }
}