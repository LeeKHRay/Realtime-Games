using System.Collections.Concurrent;

namespace RealtimeGames.Server.Models
{
    public class GameHubPlayers
    {
        private readonly ConcurrentDictionary<string, bool> _isPlayerWaiting = new();
        private readonly ConcurrentDictionary<string, string> _nameToId = new();
        private readonly ConcurrentDictionary<string, string> _idToName = new();

        public void ShowUsers()
        {
            foreach (var pair in _nameToId)
            {
                Console.WriteLine($"{pair.Key} {pair.Value}");
            }
        }

        public void AddPlayer(string playerName, string connectionId)
        {
            _nameToId[playerName] = connectionId;
            _idToName[connectionId] = playerName;
            _isPlayerWaiting[playerName] = false;
        }

        public string GetId(string playerName) => _nameToId[playerName];

        public List<string> GetAllWaitingPlayerNames() => _isPlayerWaiting.Keys.Where(name => _isPlayerWaiting[name]).ToList();
        public void WaitPlayer(string playerName) => _isPlayerWaiting[playerName] = true;
        public void CancelWaiting(string playerName) => _isPlayerWaiting[playerName] = false;

        public void TryRemovePlayer(string connectionId)
        {
            if (_idToName.TryGetValue(connectionId, out string? playerName))
            {
                _nameToId.TryRemove(playerName, out string? _);
                _idToName.TryRemove(connectionId, out string? _);
                _isPlayerWaiting.TryRemove(playerName, out bool _);
            }
        }
    }
}
