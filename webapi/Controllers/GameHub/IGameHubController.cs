using webapi.Domain.PlayerConnectionDetails;

namespace webapi.Controllers.GameHub
{
    public interface IGameHubController
    {
        Task<string?> JoinGame(IPlayerConnectionDetails playerConnectionDetails);
        void OnClientDisconnected(string connectionId);
        Task SwapPlayerPosition(string connectionId, string playerName);
        Task DeclareReady(string connectionId, bool ready);
        Task OnBid(string connectionId, int bid, bool alone);
        Task DeclareTrump(string connectionId, int trumpSuitIndex, bool alone);
        Task PlayCard(string connectionId, int sentCardId);
        Task CollectTrick(string connectionId, bool updateClients);
    }
}
