using static webapi.Domain.Statics.Enums;

namespace webapi.Domain.Tricks
{
    public class TrickCard
    {
        public Suit Suit { get; set; }
        public Rank Rank { get; set; }
        public int Id { get; set; }
        public int PlayedIndex { get; set; }

        public TrickCard(int id, Suit suit, Rank rank, int playedIndex)
        {
            Id = id;
            Suit = suit;
            Rank = rank;
            PlayedIndex = playedIndex;
        }
    }
}
