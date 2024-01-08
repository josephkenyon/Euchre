namespace webapi.Domain.PlayerState
{
    public class PlayerState
    {
        public string Name { get; set; }
        public int TeamIndex { get; set; }
        public List<Card> Hand { get; set; }
        public bool HighlightPlayer { get; set; }
        public bool IsReady { get; set; }
        public bool ShowReady { get; set; }
        public bool Passed { get; set; }
        public bool IsDealer { get; set; }

        public PlayerState()
        {
            Name = "";
            Hand = new List<Card>();
        }
    }
}
