using RealtimeGames.Shared.Models;

namespace RealtimeGames.Client.Services;

public class PlayerState : IPlayerState
{
    public Player Player { get; set; } = default!;
    public Player? Opponent { get; set; }
    public bool IsPlayer1 { get; set; }
    public string? RoomId { get; set; }

    public bool IsWaitingPlayer { get; private set; } = false;
    public bool IsJoinedGame => !string.IsNullOrEmpty(RoomId);

    public void WaitPlayer() => IsWaitingPlayer = true;

    public void CancelWaiting() => IsWaitingPlayer = false;

    public void StartGame(Player opponent, bool isPlayer1, string roomId)
    {
        IsWaitingPlayer = false;
        IsPlayer1 = isPlayer1;
        RoomId = roomId;
        Opponent = opponent;
    }

    public void QuitGame()
    {
        RoomId = null;
        Opponent = null;
    }

    public void Reset()
    {
        IsWaitingPlayer = false;
        Opponent = null;
        RoomId = null;
    }
}