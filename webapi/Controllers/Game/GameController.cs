using System.Security.Cryptography;
using webapi.Domain.Game;
using webapi.Domain.GameDetails;
using webapi.Repositories.Game;

namespace webapi.Controllers.Game
{
    public class GameController : IGameController
    {
        private readonly IGameRepository _gameRepository;

        public GameController(
            IGameRepository gameRepository
        ) {
            _gameRepository = gameRepository;
        }

        public void CreateGame(IGameDetails gameDetails)
        {
            var gameName = gameDetails.GetGameName();
            var game = new Domain.Game.Game(gameName);
            _gameRepository.AddGame(game);
        }

        public void DeleteGame(IGameDetails gameDetails)
        {
            _gameRepository.DeleteGame(gameDetails);
        }

        public IGame? GetGame(IGameDetails gameDetails)
        {
            var gameName = gameDetails.GetGameName();
            return _gameRepository.GetGame(gameName);
        }

        public void UpdateGame(IGameDetails gameDetails, IGame game)
        {
            var gameInstance = game as Domain.Game.Game ?? throw new InvalidCastException();
            _gameRepository.UpdateGame(gameDetails, gameInstance);
        }
    }
}
