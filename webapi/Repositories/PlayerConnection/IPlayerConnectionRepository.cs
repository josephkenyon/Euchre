using webapi.Domain.GameDetails;
using webapi.Domain.PlayerConnectionDetails;
using webapi.Domain.PlayerDetails;

namespace webapi.Repositories.PlayerConnection
{
    public interface IPlayerConnectionRepository
    {
        void CreateConnection(IPlayerConnectionDetails playerConnection);
        void DeleteConnection(string connectionId);
        IPlayerConnectionDetails? GetPlayerConnection(IPlayerDetails playerDetails);
        IPlayerDetails? GetPlayerDetails(string connectionId);
        IEnumerable<IPlayerConnectionDetails> GetPlayerConnections(IGameDetails gameDetails);
    }
}
