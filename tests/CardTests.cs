using webapi.Domain.Statics;
using webapi.Domain.Tricks;
using static webapi.Domain.Statics.Enums;

namespace tests
{
    public class CardTests
    {
        [Test]
        public void TrumpBeatsNonTrump()
        {
            var trumpSuit = Suit.Spade;
            var ledSuit = Suit.Heart;

            var card1 = new TrickCard(0, Suit.Heart, Rank.Queen, 0);
            var card2 = new TrickCard(0, Suit.Spade, Rank.Ace, 1);

            var winningCard = Utils.CompareCards(trumpSuit, ledSuit, card1, card2);

            Assert.That(winningCard, Is.EqualTo(-1));
        }

        [Test]
        public void NonLedSuitLosesToAll()
        {
            var trumpSuit = Suit.Spade;
            var ledSuit = Suit.Heart;

            var card1 = new TrickCard(0, Suit.Club, Rank.Ace, 0);
            var card2 = new TrickCard(0, Suit.Heart, Rank.Nine, 1);

            var winningCard = Utils.CompareCards(trumpSuit, ledSuit, card1, card2);

            Assert.That(winningCard, Is.EqualTo(-1));
        }

        [Test]
        public void NonLedSuitsAreEqual()
        {
            var trumpSuit = Suit.Spade;
            var ledSuit = Suit.Heart;

            var card1 = new TrickCard(0, Suit.Club, Rank.Ace, 0);
            var card2 = new TrickCard(0, Suit.Diamond, Rank.Nine, 1);

            var winningCard = Utils.CompareCards(trumpSuit, ledSuit, card1, card2);

            Assert.That(winningCard, Is.EqualTo(0));
        }

        [Test]
        public void SuitsAreEqualCheckRank()
        {
            var trumpSuit = Suit.Spade;
            var ledSuit = Suit.Heart;

            var card1 = new TrickCard(0, Suit.Heart, Rank.Ace, 0);
            var card2 = new TrickCard(0, Suit.Heart, Rank.Nine, 1);

            var winningCard = Utils.CompareCards(trumpSuit, ledSuit, card1, card2);

            Assert.That(winningCard, Is.EqualTo(1));
        }

        [Test]
        public void SuitsAreEqualCheckRankTrump()
        {
            var trumpSuit = Suit.Spade;
            var ledSuit = Suit.Heart;

            var card1 = new TrickCard(0, Suit.Spade, Rank.Queen, 0);
            var card2 = new TrickCard(0, Suit.Spade, Rank.Jack, 1);

            var winningCard = Utils.CompareCards(trumpSuit, ledSuit, card1, card2);

            Assert.That(winningCard, Is.EqualTo(1));
        }

        [Test]
        public void CardsAreEqualCheckOrder()
        {
            var trumpSuit = Suit.Spade;
            var ledSuit = Suit.Heart;

            var card1 = new TrickCard(0, Suit.Spade, Rank.Queen, 0);
            var card2 = new TrickCard(0, Suit.Spade, Rank.Queen, 1);

            var winningCard = Utils.CompareCards(trumpSuit, ledSuit, card1, card2);

            Assert.That(winningCard, Is.EqualTo(1));
        }
    }
}