using webapi.Data;
using webapi.Domain.GameDetails;
using webapi.Domain.Player;
using webapi.Domain.PlayerDetails;

namespace webapi.Repositories.Player
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly ILogger<PlayerRepository> _logger;
        private readonly GameContext _gameContext;

        public PlayerRepository(ILogger<PlayerRepository> logger, GameContext gameContext)
        {
            _logger = logger;
            _gameContext = gameContext;
        }

        public void CreatePlayer(IPlayerDetails playerDetails)
        {
            var gameName = playerDetails.GetGameName();
            var playerName = playerDetails.GetPlayerName();

            var playerCount = GetPlayers(playerDetails).Count();
            var player = new Domain.Player.Player(gameName, playerName)
            {
                PlayerIndex = playerCount
            };

            _gameContext.Players.Add(player);
            _gameContext.SaveChanges();
        }

        public IPlayer? GetPlayer(IPlayerDetails playerDetails)
        {
            var gameName = playerDetails.GetGameName();
            var playerName = playerDetails.GetPlayerName();

            try
            {
                return _gameContext.Players.SingleOrDefault(player => player.GameName == gameName && player.Name == playerName);
            }
            catch (Exception)
            {
                _logger.LogError("Error retrieving a player with '{GameName}' && '{PlayerName}'", gameName, playerName);
                return null;
            }
        }

        public IEnumerable<IPlayer> GetPlayers(IGameDetails gameDetails)
        {
            var gameName = gameDetails.GetGameName();

            return _gameContext.Players.Where(player => player.GameName == gameName);
        }

        public void UpdatePlayer(IPlayerDetails playerDetails, Domain.Player.Player player)
        {
            var gameName = playerDetails.GetGameName();
            var playerName = playerDetails.GetPlayerName();
            var playersList = _gameContext.Players.Where(game => game.GameName == gameName).ToList();
            var index = playersList.FindIndex(player => player.Name == playerName);

            playersList[index] = player;

            _gameContext.SaveChanges();
        }

        public void UpdatePlayers(IGameDetails gameDetails, IEnumerable<Domain.Player.Player> playerList)
        {
            var gameName = gameDetails.GetGameName();
            var playersList = _gameContext.Players.Where(game => game.GameName == gameName).ToList();

            foreach (var player in playerList)
            {
                var index = playersList.FindIndex(p => p.Name == player.Name);

                playersList[index] = player;
            }

            _gameContext.SaveChanges();
        }
    }
}
