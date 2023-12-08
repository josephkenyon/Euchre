using webapi.Domain.GameDetails;
using webapi.Domain.Tricks;
using webapi.Repositories.Trick;
using static webapi.Domain.Statics.Enums;

namespace webapi.Controllers.Trick
{
    public class TrickController : ITrickController
    {
        private readonly ITrickRepository _trickRepository;

        public TrickController(
            ITrickRepository trickRepository
        )
        {
            _trickRepository = trickRepository;
        }

        public ITrick? GetTrick(IGameDetails gameDetails)
        {
            return _trickRepository.GetTrick(gameDetails);
        }

        public void CreateNewTrick(IGameDetails gameDetails, Suit trumpSuit, Suit ledSuit)
        {
            var trick = new Domain.Tricks.Trick
            {
                GameName = gameDetails.GetGameName(),
                TrumpSuit = trumpSuit,
                LedSuit = ledSuit
            };

            _trickRepository.AddTrick(trick);
        }

        public void DeleteTrick(IGameDetails gameDetails)
        {
            _trickRepository.DeleteTrick(gameDetails);
        }
    }
}
