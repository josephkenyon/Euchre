using webapi.Domain;
using webapi.Domain.Statics;
using static webapi.Domain.Statics.Enums;

namespace tests
{
    public class CardFromIdTests
    {
        [Test]
        public void WinningCardInSuit()
        {
            var cards = new List<Card>();

            var index = 0;
            Enum.GetValues<Suit>().ToList().ForEach(suit =>
            {
                Enum.GetValues<Rank>().ToList().ForEach(rank =>
                {
                    var card = new Card(index++, suit, rank);
                    var card2 = new Card(index++, suit, rank);

                    var testCard = Utils.GetCardFromId(card.Id);
                    var testCard2 = Utils.GetCardFromId(card2.Id);

                    Assert.Multiple(() =>
                    {
                        Assert.That(testCard.Suit, Is.EqualTo(card.Suit));
                        Assert.That(testCard.Rank, Is.EqualTo(card.Rank));
                        Assert.That(testCard2.Suit, Is.EqualTo(card2.Suit));
                        Assert.That(testCard2.Rank, Is.EqualTo(card2.Rank));
                    });
                });
            });

        }

    }
}