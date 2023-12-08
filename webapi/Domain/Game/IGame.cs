using webapi.Domain.Bidding;
using webapi.Domain.GameDetails;
using static webapi.Domain.Statics.Enums;

namespace webapi.Domain.Game
{
    public interface IGame
    {
        IGameDetails GetGameDetails();
        int GetCurrentBid();
        int GetDealerIndex();
        int GetPlayerTurnIndex();
        void SetPlayerTurnIndex(int index);
        int GetTookBidTeamIndex();
        void SetTookBidTeamIndex(int newTeamIndex);
        void LonerCalled(int ignorePlayerIndex);
        void FlipDealerCard();
        Phase GetPhase();
        Suit GetTrumpSuit();
        Card GetTrumpCard()
        void SetTrumpSuit(Suit suit);
        void IncrementPhase();
        void SkipPhase();
        void DealRoundStart(List<Card> kitty, Card trumpCard);
        void StartNewRound();
        void AddScore(int teamIndex, int scoreIncrementValue);
        List<string> GetScoreLog(string teamName, int teamIndex);
        int GetTotalScore(int teamIndex);
        List<RoundBidResult> GetRoundBidResults();
        void AddRoundBidResult(Suit trumpSuit, int teamIndex, int bid);
        List<int> GetCardIds(int teamIndex);
        void AddCardIds(int teamIndex, List<int> cardIds);
        void SetCurrentBid(int bid);
        void IncrementPlayerTurnIndex();
    }
}
