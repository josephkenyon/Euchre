namespace webapi.Domain.PlayerState
{
    public class PlayerState
    {
        public string Name { get; set; }
        public int TeamIndex { get; set; }
        public List<Card> Hand { get; set; }
        public List<Card> DisplayedCards { get; set; }
        public bool HighlightPlayer { get; set; }
        public bool IsReady { get; set; }
        public bool ShowReady { get; set; }
        public int LastBid { get; set; }
        public bool ShowLastBid { get; set; }

        public PlayerState()
        {
            Name = "";
            Hand = new List<Card>();
            DisplayedCards = new List<Card>();
        }
    }
}
