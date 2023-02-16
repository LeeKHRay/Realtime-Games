using RealtimeGames.Client.Services.GameStates;

namespace RealtimeGames.Client.Services;

public interface IGameHubConnection<T> where T : IGameState
{
    bool IsConnected { get; }

    void CreateConnection(string gamename, string playerName, Action StateHasChanged);
    Task StartAsync();
    Task WaitPlayer();
    Task CancelWaiting();
    Task QuitGame();
    ValueTask CloseConnection();

    void On(string methodName, Action handler);
    void On<T1>(string methodName, Action<T1> handler);
    void On<T1, T2>(string methodName, Action<T1, T2> handler);

    Task SendAsync(string methodName, object? arg1);
    Task SendAsync(string methodName, object? arg1, object? arg2);
    Task SendAsync(string methodName, object? arg1, object? arg2, object? arg3);
}