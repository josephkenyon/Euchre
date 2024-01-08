using webapi.Domain.PlayerDetails;
using webapi.Domain.Statics;

namespace webapi.Domain.Player
{
    public interface IPlayer
    {
        IPlayerDetails GetPlayerDetails();
        int GetIndex();
        int GetTeamIndex();
        string GetName();
        string GetGameName();
        bool GetPassed();
        bool GetIsReady();
        void SetIsReady(bool ready);
        void RemoveCard(int id);
        List<Card> GetHand();
        void SortHand(Enums.Suit? trumpSuit);
        void DealCards(List<Card> cards);
        void DealCard(Card card);
        void ResetBiddingState();
        void Pass();
        void GoUnder(List<Card> cards, string goingUnderCardsIds);
        bool CanGoUnder();
    }
}
