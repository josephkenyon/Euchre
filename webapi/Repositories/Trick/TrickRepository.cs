using webapi.Data;
using webapi.Domain.GameDetails;
using webapi.Domain.Tricks;

namespace webapi.Repositories.Trick
{
    public class TrickRepository : ITrickRepository
    {
        private readonly ILogger<TrickRepository> _logger;
        private readonly GameContext _gameContext;

        public TrickRepository(ILogger<TrickRepository> logger, GameContext gameContext)
        {
            _logger = logger;
            _gameContext = gameContext;
        }

        public ITrick? GetTrick(IGameDetails gameDetails)
        {
            var gameName = gameDetails.GetGameName();

            try
            {
                return _gameContext.Tricks.SingleOrDefault(trick => trick.GameName == gameName);
            }
            catch (Exception)
            {
                _logger.LogError("Error retrieving a trick with '{GameName}'", gameName);
                return null;
            }
        }

        public void AddTrick(Domain.Tricks.Trick trick)
        {
            _gameContext.Tricks.Add(trick);
            _gameContext.SaveChanges();
        }

        public void DeleteTrick(IGameDetails gameDetails)
        {
            var gameName = gameDetails.GetGameName();

            try
            {
                var trick = _gameContext.Tricks.Single(trick => trick.GameName == gameName);

                _gameContext.Tricks.Remove(trick);
                _gameContext.SaveChanges();
            }
            catch (Exception)
            {
                _logger.LogError("Error deleting the trick for '{GameName}'", gameName);
            }
        }
    }
}
