using webapi.Domain.Game;
using webapi.Domain.Player;
using webapi.Domain.Tricks;
using static webapi.Domain.Statics.Enums;

namespace webapi.Domain.Statics
{
    public static class Utils
    {
        public static int GetWinningCardId(Suit trumpSuit, List<TrickCard> cards)
        {
            var ledSuit = cards.OrderBy(card => card.PlayedIndex).First().Suit;

            var sortedCards = new List<TrickCard>(cards);

            sortedCards.Sort((a, b) => CompareCards(trumpSuit, ledSuit, a, b));

            return sortedCards.Last().Id;
        }

        public static List<Card> GetValidPlays(List<TrickCard> playedCards, List<Card> hand, Suit trumpSuit)
        {
            var ledTrickCard = playedCards.OrderBy(card => card.PlayedIndex).First();

            var ledSuit = GetFunctionalSuit(new Card(0, ledTrickCard.Suit, ledTrickCard.Rank), trumpSuit);
            var hasSuit = hand.Any(card => GetFunctionalSuit(card, trumpSuit) == ledSuit);

            if (hasSuit)
            {
                return hand.Where(card => GetFunctionalSuit(card, trumpSuit) == ledSuit).ToList();
            }

            return hand;
        }

        private static Suit GetFunctionalSuit(Card card, Suit trumpSuit)
        {
            if (card.Rank != Rank.Jack)
            {
                return card.Suit;
            }

            var littleBoySuit = GetLittleBoySuit(trumpSuit);
            if (card.Suit == littleBoySuit)
            {
                return trumpSuit;
            }

            return card.Suit;
        }

        public static void StartNewRound(IGame game, IEnumerable<IPlayer> players)
        {
            game.StartNewRound();

            DealCards(game, players);
        }

        public static void DealCards(IGame game, IEnumerable<IPlayer> players)
        {
            var deck = new List<Card>();

            var index = 0;
            var rng = new Random();
            var shuffleCount = rng.Next(20, 30);

            Enum.GetValues<Suit>().ToList().ForEach(suit =>
            {
                Enum.GetValues<Rank>().ToList().ForEach(rank =>
                {
                    deck.Add(new Card(index++, suit, rank));
                });
            });

            for (int i = 0; i < shuffleCount; i++)
            {
                deck = deck.OrderBy(card => rng.Next()).ToList();
            }

            index = 0;
            var startingIndex = 0;
            foreach (var player in players)
            {
                var cards = new List<Card>();
                for (int i = startingIndex; i < startingIndex + 5; i++)
                {
                    cards.Add(deck[i]);
                }

                index++;
                if (index > 3)
                {
                    index = 0;  
                }

                cards.Sort((a, b) =>
                {
                    if ((int)a.Suit > (int)b.Suit)
                    {
                        return 1;
                    }
                    else if ((int)a.Suit < (int)b.Suit)
                    {
                        return -1;
                    }

                    if ((int)a.Rank > (int)b.Rank)
                    {
                        return 1;
                    }
                    else if ((int)a.Rank < (int)b.Rank)
                    {
                        return -1;
                    }

                    return 0;
                });

                player.DealCards(cards);

                startingIndex += 5;
            }

            List<Card> kitty = new();
            for (int i = startingIndex; i < startingIndex + 3; i++)
            {
                kitty.Add(deck[i]);
            }

            game.DealRoundStart(kitty, deck[startingIndex + 3]);
        }

        public static List<Card> GetCardsFromString(string? handString)
        {
            var cardsList = new List<Card>();

            if (handString == null)
            {
                return cardsList;
            }

            var cardStrings = handString.Split(";").ToList();
            cardStrings.RemoveAll(string.IsNullOrEmpty);
                
            cardsList.AddRange(cardStrings.Select(GetCardFromString));

            return cardsList;
        }

        public static int GetTeamIndex(int playerIndex)
        {
            return (playerIndex == 0 || playerIndex == 2) ? 0 : 1;
        }

        public static Card GetCardFromString(string cardString)
        {
            var cardStringList = cardString.Split(":");

            _ = Enum.TryParse(cardStringList[1], out Suit suit);
            _ = Enum.TryParse(cardStringList[2], out Rank rank);

            return new Card(int.Parse(cardStringList[0]), suit, rank);
        }

        public static string StringifyCards(List<Card> cards)
        {
            var cardStringList = new List<string>();
            cardStringList.AddRange(cards.Select(StringifyCard));

            return string.Join(";", cardStringList);
        }

        public static string StringifyCard(Card card)
        {
            return $"{card.Id}:{card.Suit}:{card.Rank}";
        }

        public static int CompareCards(Suit trumpSuit, Suit ledSuit, TrickCard card1, TrickCard card2)
        {
            var suitOneValue = GetSuitValue(trumpSuit, ledSuit, card1.Suit);
            var suitTwoValue = GetSuitValue(trumpSuit, ledSuit, card2.Suit);

            if (IsBigBoy(trumpSuit, card1.Suit, card1.Rank))
            {
                return 1;
            }
            else if (IsBigBoy(trumpSuit, card2.Suit, card2.Rank))
            {
                return -1;
            }
            else if (IsLittleBoy(trumpSuit, card1.Suit, card1.Rank))
            {
                return 1;
            }
            else if (IsLittleBoy(trumpSuit, card2.Suit, card2.Rank))
            {
                return -1;
            }

            if (suitOneValue > suitTwoValue)
            {
                return 1;
            }
            else if (suitOneValue < suitTwoValue)
            {
                return -1;
            }

            if (card1.Suit != trumpSuit && card1.Suit != ledSuit && card2.Suit != trumpSuit && card2.Suit != ledSuit)
            {
                return 0;
            }
            else
            {
                if (card1.Rank < card2.Rank)
                {
                    return 1;
                }
                else if (card1.Rank > card2.Rank)
                {
                    return -1;
                }
            }

            return card1.PlayedIndex > card2.PlayedIndex ? -1 : 1;
        }

        public static int CompareCards(Suit? trumpSuit, Card card1, Card card2)
        {
            if (trumpSuit != null && IsBigBoy((Suit) trumpSuit, card1.Suit, card1.Rank))
            {
                return 1;
            }
            else if (trumpSuit != null && IsBigBoy((Suit) trumpSuit, card2.Suit, card2.Rank))
            {
                return -1;
            }
            else if (trumpSuit != null && IsLittleBoy((Suit) trumpSuit, card1.Suit, card1.Rank))
            {
                return 1;
            }
            else if (trumpSuit != null && IsLittleBoy((Suit) trumpSuit, card2.Suit, card2.Rank))
            {
                return -1;
            }

            if (card1.Suit == trumpSuit && card2.Suit != trumpSuit)
            {
                return 1;
            }
            else if (card2.Suit == trumpSuit && card1.Suit != trumpSuit)
            {
                return -1;
            }

            if (card1.Suit < card2.Suit)
            {
                return 1;
            } 
            else if (card2.Suit < card1.Suit)
            {
                return -1;
            }

            if (card1.Rank < card2.Rank)
            {
                return 1;
            }
            else if (card2.Rank < card1.Rank)
            {
                return -1;
            }

            return 0;
        }

        public static bool IsBigBoy(Suit trumpSuit, Suit suit, Rank rank)
        {
            return suit == trumpSuit && rank == Rank.Jack;
        }

        public static bool IsLittleBoy(Suit trumpSuit, Suit suit, Rank rank)
        {
            return suit == GetLittleBoySuit(trumpSuit) && rank == Rank.Jack;
        }

        public static Suit GetLittleBoySuit(Suit trumpSuit)
        {
            return trumpSuit switch
            {
                Suit.Heart => Suit.Diamond,
                Suit.Spade => Suit.Club,
                Suit.Diamond => Suit.Heart,
                Suit.Club => Suit.Spade,
                _ => throw new Exception("Impossible"),
            };
        }

        public static int GetSuitValue(Suit trumpSuit, Suit ledSuit, Suit suit)
        {
            var trumpWasLed = trumpSuit == ledSuit;

            if (trumpWasLed)
            {
                return suit == trumpSuit ? 1 : 0;
            }
            else
            {
                if (suit == trumpSuit)
                {
                    return 2;
                }
                else if (suit == ledSuit)
                {
                    return 1;
                }
            }

            return 0;
        }

        public static TrickState GetTrickState(ITrick trick, int playerIndex)
        {
            var trickState = new TrickState();

            if (trick == null)
            {
                return trickState;
            }

            var trickPlays = trick.GetTrickPlays();

            foreach (var trickPlay in trickPlays)
            {
                if (trickPlay.PlayerIndex == playerIndex)
                {
                    trickState.MyCard = trickPlay.Card;
                }
                else if (trickPlay.PlayerIndex == DecrementIndex(playerIndex, 1))
                {
                    trickState.RightOpponentCard = trickPlay.Card;
                }
                else if (trickPlay.PlayerIndex == DecrementIndex(playerIndex, 2))
                {
                    trickState.AllyCard = trickPlay.Card;
                }
                else if (trickPlay.PlayerIndex == DecrementIndex(playerIndex, 3))
                {
                    trickState.LeftOpponentCard = trickPlay.Card;
                }
            }

            return trickState;
        }

        public static int DecrementIndex(int index, int amount = 1)
        {
            var newIndex = index;
            newIndex -= amount;
            if (newIndex < 0)
            {
                newIndex += 4;
            }

            return newIndex;
        }

        public static int IncrementIndex(int index, int amount = 1)
        {
            var newIndex = index;
            newIndex += amount;
            if (newIndex > 3)
            {
                newIndex -= 4;
            }

            return newIndex;
        }

        public static Card GetCardFromId(int id)
        {
            var suit = (Suit)(id / 6);
            var rank = (Rank)(id - (int)suit * 6);

            return new Card(id, suit, rank);
        }
    }
}