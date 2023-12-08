using static webapi.Domain.Statics.Enums;

namespace webapi.Domain
{
    public class TestCard
    {
        public Suit Suit { get; set; }
        public Rank Rank { get; set; }

        public TestCard()
        {
        }

        public TestCard(Suit suit, Rank rank)
        {
            Suit = suit;
            Rank = rank;
        }

        public Card ToCard()
        {
            return new Card(-1, Suit, Rank);
        }
    }
}
