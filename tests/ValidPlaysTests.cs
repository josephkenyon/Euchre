using webapi.Domain;
using webapi.Domain.Statics;
using webapi.Domain.Tricks;
using static webapi.Domain.Statics.Enums;

namespace tests
{
    public class ValidPlaysTests
    {
        [Test]
        public void MustPlayInSuit()
        {
            var trumpSuit = Suit.Spade;

            var playedCards = new List<TrickCard>
            {
                new TrickCard(0, Suit.Heart, Rank.Queen, 1),
                new TrickCard(1, Suit.Heart, Rank.King, 2)
            };

            var hand = new List<Card>
            {
                new Card(2, Suit.Heart, Rank.Ace),
                new Card(3, Suit.Diamond, Rank.Ace)
            };

            var validPlays = Utils.GetValidPlays(playedCards, hand, trumpSuit);

            Assert.Multiple(() =>
            {
                Assert.That(validPlays, Has.Count.EqualTo(1));
                Assert.That(validPlays[0].Suit, Is.EqualTo(Suit.Heart));
            });
        }

        [Test]
        public void CanPlayAnythingIfVoid()
        {
            var trumpSuit = Suit.Spade;

            var playedCards = new List<TrickCard>
            {
                new TrickCard(0, Suit.Heart, Rank.Queen, 1),
                new TrickCard(1, Suit.Heart, Rank.King, 2)
            };

            var hand = new List<Card>
            {
                new Card(2, Suit.Club, Rank.Ace),
                new Card(3, Suit.Club, Rank.Ace)
            };

            var validPlays = Utils.GetValidPlays(playedCards, hand, trumpSuit);

            Assert.Multiple(() =>
            {
                Assert.That(validPlays, Has.Count.EqualTo(2));
            });
        }

        [Test]
        public void MustTrumpIfVoid()
        {
            var trumpSuit = Suit.Spade;

            var playedCards = new List<TrickCard>
            {
                new TrickCard(0, Suit.Heart, Rank.Queen, 1),
                new TrickCard(1, Suit.Heart, Rank.King, 2)
            };

            var hand = new List<Card>
            {
                new Card(2, Suit.Spade, Rank.Jack),
                new Card(3, Suit.Club, Rank.Ace)
            };

            var validPlays = Utils.GetValidPlays(playedCards, hand, trumpSuit);

            Assert.Multiple(() =>
            {
                Assert.That(validPlays, Has.Count.EqualTo(1));
                Assert.That(validPlays[0].Rank, Is.EqualTo(Rank.Jack));
            });
        }

        [Test]
        public void MustWinIfYouCan()
        {
            var trumpSuit = Suit.Spade;

            var playedCards = new List<TrickCard>
            {
                new TrickCard(0, Suit.Heart, Rank.Queen, 1),
                new TrickCard(1, Suit.Heart, Rank.King, 2)
            };

            var hand = new List<Card>
            {
                new Card(2, Suit.Heart, Rank.Ace),
                new Card(3, Suit.Heart, Rank.Jack)
            };

            var validPlays = Utils.GetValidPlays(playedCards, hand, trumpSuit);

            Assert.Multiple(() => 
            {
                Assert.That(validPlays, Has.Count.EqualTo(1));
                Assert.That(validPlays[0].Rank, Is.EqualTo(Rank.Ace));
            });

        }

        [Test]
        public void MustWinIfYouCanTrump()
        {
            var trumpSuit = Suit.Spade;

            var playedCards = new List<TrickCard>
            {
                new TrickCard(0, Suit.Heart, Rank.Ace, 1),
                new TrickCard(1, Suit.Heart, Rank.King, 2)
            };

            var hand = new List<Card>
            {
                new Card(2, Suit.Spade, Rank.Ace),
                new Card(3, Suit.Heart, Rank.Ace)
            };

            var validPlays = Utils.GetValidPlays(playedCards, hand, trumpSuit);

            Assert.Multiple(() =>
            {
                Assert.That(validPlays, Has.Count.EqualTo(1));
                Assert.That(validPlays[0].Suit, Is.EqualTo(Suit.Spade));
            });
        }
    }
}