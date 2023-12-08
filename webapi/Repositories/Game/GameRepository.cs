using webapi.Data;
using webapi.Domain.Game;
using webapi.Domain.GameDetails;

namespace webapi.Repositories.Game
{
    public class GameRepository : IGameRepository
    {
        private readonly ILogger<GameRepository> _logger;
        private readonly GameContext _gameContext;

        public GameRepository(ILogger<GameRepository> logger, GameContext gameContext)
        {
            _logger = logger;
            _gameContext = gameContext;
        }

        public void AddGame(Domain.Game.Game game)
        {
            _gameContext.Add(game);
            _gameContext.SaveChanges();
        }

        public void DeleteGame(IGameDetails gameDetails)
        {
            var gameName = gameDetails.GetGameName();

            try
            {
                var game = _gameContext.Games.SingleOrDefault(game => game.Name == gameName);

                if (game != null)
                {
                    _gameContext.Games.Remove(game);
                    _gameContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                _logger.LogError("Error deleting a game with '{GameName}'", gameName);
                return;
            }
        }


        public IGame? GetGame(string gameName)
        {
            try
            {
                return _gameContext.Games.SingleOrDefault(game => game.Name == gameName);
            }
            catch (Exception)
            {
                _logger.LogError("Error retrieving a game with '{GameName}'", gameName);
                return null;
            }
        }

        public void UpdateGame(IGameDetails gameDetails, Domain.Game.Game game)
        {
            var gameName = gameDetails.GetGameName();
            var gamesList = _gameContext.Games.ToList();
            var index = gamesList.FindIndex(game => game.Name == gameName);

            gamesList[index] = game;

            _gameContext.SaveChanges();
        }
    }
}
