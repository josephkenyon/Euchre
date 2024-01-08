using static webapi.Domain.Statics.Enums;

namespace webapi.Domain.Bidding
{
    public class RoundBidResult
    {
        public Suit TrumpSuit { get; set; }
        public int TeamIndex { get; set; }

        public RoundBidResult(Suit trumpSuit, int teamIndex)
        {
            TrumpSuit = trumpSuit;
            TeamIndex = teamIndex;
        }
    }
}
