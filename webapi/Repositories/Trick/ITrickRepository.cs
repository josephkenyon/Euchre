using webapi.Domain.GameDetails;
using webapi.Domain.Tricks;

namespace webapi.Repositories.Trick
{
    public interface ITrickRepository
    {
        public ITrick? GetTrick(IGameDetails gameDetails);
        void AddTrick(Domain.Tricks.Trick trick);
        void DeleteTrick(IGameDetails gameDetails);
    }
}
