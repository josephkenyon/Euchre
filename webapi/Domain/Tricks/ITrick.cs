using static webapi.Domain.Statics.Enums;

namespace webapi.Domain.Tricks
{
    public interface ITrick
    {
        Suit GetTrumpSuit();
        public void PlayCard(Card card, int playerId);
        public List<Card> GetCards();
        public List<TrickPlay> GetTrickPlays();
    }
}
