using webapi.Domain.GameDetails;
using webapi.Domain.Tricks;
using static webapi.Domain.Statics.Enums;

namespace webapi.Controllers.Trick
{
    public interface ITrickController
    {
        public ITrick? GetTrick(IGameDetails gameDetails);
        void CreateNewTrick(IGameDetails gameDetails, Suit trumpSuit, Suit ledSuit);
        void DeleteTrick(IGameDetails gameDetails);
    }
}
