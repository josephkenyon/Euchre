using webapi.Domain.Tricks;
using static webapi.Domain.Statics.Enums;

namespace webapi.Domain
{
    public class Card
    {
        public int Id { get; set; }
        public Suit Suit { get; set; }
        public Rank Rank { get; set; }

        public Card(int id, Suit suit, Rank rank)
        {
            Id = id;
            Suit = suit;
            Rank = rank;
        }

        public TrickCard ToTrickCard() => new(0, Suit, Rank, 4);
    }
}
