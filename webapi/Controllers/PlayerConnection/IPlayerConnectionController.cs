using webapi.Domain.GameDetails;
using webapi.Domain.PlayerConnectionDetails;
using webapi.Domain.PlayerDetails;

namespace webapi.Controllers.PlayerConnection
{
    public interface IPlayerConnectionController
    {
        Task<string?> JoinGame(IPlayerConnectionDetails playerConnectionDetails);
        int GetConnectedPlayerCount(IGameDetails gameDetails);
        IPlayerDetails? GetPlayerDetails(string connectionId);
        Task UpdateClients(IGameDetails gameDetails);
        Task MessageClients(IGameDetails gameDetails, string content, int teamIndex);
        Task MessageClient(IPlayerDetails playerDetails, string content, int teamIndex);
        Task MessageErrorToClients(IGameDetails playerDetails, string errorMessage);
        Task MessageErrorToClient(IPlayerDetails playerDetails, string errorMessage);
        void RemoveConnection(string connectionId);
    }
}
