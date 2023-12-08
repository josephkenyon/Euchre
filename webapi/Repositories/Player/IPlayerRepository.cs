using webapi.Domain.GameDetails;
using webapi.Domain.Player;
using webapi.Domain.PlayerDetails;

namespace webapi.Repositories.Player
{
    public interface IPlayerRepository
    {
        public void CreatePlayer(IPlayerDetails playeDetails);
        public IEnumerable<IPlayer> GetPlayers(IGameDetails gameDetails);
        public IPlayer? GetPlayer(IPlayerDetails playeDetails);
        public void UpdatePlayer(IPlayerDetails playerDetails, Domain.Player.Player player);
        public void UpdatePlayers(IGameDetails gameDetails, IEnumerable<Domain.Player.Player> playerList);
    }
}
