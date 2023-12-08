using webapi.Domain.PlayerDetails;
using webapi.Domain.Statics;

namespace webapi.Domain.Player
{
    public class Player : IPlayer
    {
        public IPlayerDetails GetPlayerDetails() => new PlayerDetails.PlayerDetails(GameName, Name);
        public string Name { get; set; }
        public string GameName { get; set; }
        public int PlayerIndex { get; set; }
        public bool Passed { get; set; }
        public bool Ready { get; set; }
        public string? HandString { get; set; }

        public Player(string gameName, string name)
        {
            GameName = gameName;
            Name = name;
        }

        public int GetIndex() => PlayerIndex;

        public int GetTeamIndex() => (PlayerIndex == 0 || PlayerIndex == 2) ? 0 : 1;

        public bool GetIsReady() => Ready;
        public void SetIsReady(bool ready)
        {
            Ready = ready;
        }

        public bool GetPassed() => Passed;

        public string GetName() => Name;

        public string GetGameName() => GameName;

        public List<Card> GetHand()
        {
            return Utils.GetCardsFromString(HandString);
        }

        public void DealCards(List<Card> cards)
        {
            HandString = Utils.StringifyCards(cards);
        }

        public void DealCard(Card card)
        {
            var hand = GetHand();
            hand.Add(card);

            HandString = Utils.StringifyCards(hand);
        }

        public void RemoveCard(int id)
        {
            var cardsList = new List<Card>();

            if (HandString == null)
            {
                throw new InvalidOperationException();
            }

            var cardStringList = HandString.Split(";").ToList();

            var stringToDelete = cardStringList.Single(cardString => cardString.StartsWith($"{id}:"));

            cardStringList.Remove(stringToDelete);

            HandString = string.Join(";", cardStringList);
        }
    }
}
