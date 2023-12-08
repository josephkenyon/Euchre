using webapi.Domain.Game;
using webapi.Domain.GameDetails;

namespace webapi.Repositories.Game
{
    public interface IGameRepository
    {
        public void AddGame(Domain.Game.Game game);
        public void DeleteGame(IGameDetails gameDetails);
        public IGame? GetGame(string gameName);
        void UpdateGame(IGameDetails gameDetails, Domain.Game.Game game);
    }
}
