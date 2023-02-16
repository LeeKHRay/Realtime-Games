using RealtimeGames.Shared.Models;

namespace RealtimeGames.Client.Services;

public interface IPlayerState
{
    Player Player { get; set; }
    Player? Opponent { get; set; }
    bool IsPlayer1 { get; set; }
    string? RoomId { get; set; }

    bool IsWaitingPlayer { get; }
    bool IsJoinedGame { get; }

    void WaitPlayer();

    void CancelWaiting();

    void StartGame(Player opponent, bool isPlayer1, string roomId);

    void QuitGame();

    void Reset();
}