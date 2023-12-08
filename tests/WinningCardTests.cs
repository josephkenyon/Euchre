using webapi.Domain.Statics;
using webapi.Domain.Tricks;
using static webapi.Domain.Statics.Enums;

namespace tests
{
    public class WinningCardTests
    {
        [Test]
        public void WinningCardInSuit()
        {
            var trumpSuit = Suit.Spade;

            var cards = new List<TrickCard>
            {
                new TrickCard(0, Suit.Heart, Rank.Ten, 0),
                new TrickCard(1, Suit.Heart, Rank.Queen, 1),
                new TrickCard(2, Suit.Heart, Rank.Ace, 2),
                new TrickCard(3, Suit.Heart, Rank.Jack, 3),
            };

            var winningCardId = Utils.GetWinningCardId(trumpSuit, cards);

            Assert.That(winningCardId, Is.EqualTo(2));
        }

        [Test]
        public void WinningCardInSuitTrump()
        {
            var trumpSuit = Suit.Spade;

            var cards = new List<TrickCard>
            {
                new TrickCard(0, Suit.Heart, Rank.Queen, 0),
                new TrickCard(1, Suit.Spade, Rank.King, 1),
                new TrickCard(2, Suit.Heart, Rank.Ace, 2),
                new TrickCard(3, Suit.Heart, Rank.Ten, 3),
            };

            var winningCardId = Utils.GetWinningCardId(trumpSuit, cards);

            Assert.That(winningCardId, Is.EqualTo(1));
        }

        [Test]
        public void WinningCardByOrder()
        {
            var trumpSuit = Suit.Spade;

            var cards = new List<TrickCard>
            {
                new TrickCard(0, Suit.Heart, Rank.Ace, 0),
                new TrickCard(1, Suit.Heart, Rank.Queen, 1),
                new TrickCard(2, Suit.Heart, Rank.Ace, 2),
                new TrickCard(3, Suit.Heart, Rank.Ten, 3),
            };

            var winningCardId = Utils.GetWinningCardId(trumpSuit, cards);

            Assert.That(winningCardId, Is.EqualTo(0));
        }


        [Test]
        public void WinningCardBySuit()
        {
            var trumpSuit = Suit.Spade;

            var cards = new List<TrickCard>
            {
                new TrickCard(0, Suit.Heart, Rank.Nine, 0),
                new TrickCard(1, Suit.Club, Rank.Queen, 1),
                new TrickCard(2, Suit.Club, Rank.Ace, 2),
                new TrickCard(3, Suit.Diamond, Rank.Ten, 3),
            };

            var winningCardId = Utils.GetWinningCardId(trumpSuit, cards);

            Assert.That(winningCardId, Is.EqualTo(0));
        }
    }
}