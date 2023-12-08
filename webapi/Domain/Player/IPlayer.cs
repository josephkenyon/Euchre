using webapi.Domain.PlayerDetails;

namespace webapi.Domain.Player
{
    public interface IPlayer
    {
        IPlayerDetails GetPlayerDetails();
        int GetIndex();
        int GetTeamIndex();
        string GetName();
        string GetGameName();
        bool GetIsReady();
        void SetIsReady(bool ready);
        void RemoveCard(int id);
        List<Card> GetHand();
        void DealCards(List<Card> cards);
        void DealCard(Card card);
    }
}
