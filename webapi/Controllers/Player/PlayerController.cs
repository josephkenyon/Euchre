using webapi.Domain.GameDetails;
using webapi.Domain.Player;
using webapi.Domain.PlayerDetails;
using webapi.Repositories.Player;

namespace webapi.Controllers.Player
{
    public class PlayerController : IPlayerController
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerController(
            IPlayerRepository playerRepository
        ) {
            _playerRepository = playerRepository;
        }

        public void CreatePlayer(IPlayerDetails playeDetails)
        {
            _playerRepository.CreatePlayer(playeDetails);
        }

        public IPlayer? GetPlayer(IPlayerDetails playeDetails) => _playerRepository.GetPlayer(playeDetails);

        public IEnumerable<IPlayer> GetPlayers(IGameDetails gameDetails) => _playerRepository.GetPlayers(gameDetails);

        public void UpdatePlayer(IPlayerDetails playerDetails, IPlayer player)
        {
            var playerInstance = player as Domain.Player.Player ?? throw new InvalidCastException();
            _playerRepository.UpdatePlayer(playerDetails, playerInstance);
        }

        public void UpdatePlayers(IGameDetails gameDetails, IEnumerable<IPlayer> players)
        {
            _playerRepository.UpdatePlayers(gameDetails, players.Select(player => (Domain.Player.Player)player));

        }

        public void ResetBidState(IGameDetails gameDetails)
        {
            var players = _playerRepository.GetPlayers(gameDetails);

            foreach (var player in players)
            {
                player.ResetBiddingState();
            }

            _playerRepository.UpdatePlayers(gameDetails, players.Select(player => (Domain.Player.Player) player));
        }

        public void SwapPlayerIndices(IPlayer player, IPlayer swapPlayer)
        {
            var players = new List<Domain.Player.Player>()
            {
                (Domain.Player.Player) player,
                (Domain.Player.Player) swapPlayer
            };

            (players[1].PlayerIndex, players[0].PlayerIndex) = (players[0].PlayerIndex, players[1].PlayerIndex);

            _playerRepository.UpdatePlayers(player.GetPlayerDetails(), players);
        }
    }
}
