using webapi.Domain.Game;
using webapi.Domain.GameDetails;
using webapi.Domain.Player;
using webapi.Domain.PlayerDetails;

namespace webapi.Controllers.Player
{
    public interface IPlayerController
    {
        public void CreatePlayer(IPlayerDetails playeDetails);
        public IEnumerable<IPlayer> GetPlayers(IGameDetails gameDetails);
        public IPlayer? GetPlayer(IPlayerDetails playeDetails);
        void UpdatePlayer(IPlayerDetails playerDetails, IPlayer player);
        void UpdatePlayers(IGameDetails gameDetails, IEnumerable<IPlayer> players);
        void ResetBidState(IGameDetails gameDetails);
        void SwapPlayerIndices(IPlayer player, IPlayer swapPlayer);
    }
}
