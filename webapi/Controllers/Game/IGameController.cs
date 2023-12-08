using webapi.Domain.Game;
using webapi.Domain.GameDetails;

namespace webapi.Controllers.Game
{
    public interface IGameController
    {
        void CreateGame(IGameDetails gameDetails);
        void DeleteGame(IGameDetails gameDetails);
        IGame? GetGame(IGameDetails gameDetails);
        void UpdateGame(IGameDetails gameDetails, IGame game);
    }
}
