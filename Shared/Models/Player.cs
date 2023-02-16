namespace RealtimeGames.Shared.Models;

public class Player
{
    public string Name { get; set; } = default!;

    public Score Score { get; set; } = default!;

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        Player? player = obj as Player;
        return Name == player?.Name;
    }
}