namespace webapi.Domain.Tricks
{
    public class TrickState
    {
        public Card? MyCard { get; set; }
        public Card? AllyCard { get; set; }
        public Card? LeftOpponentCard { get; set; }
        public Card? RightOpponentCard { get; set; }

        public TrickState()
        {

        }
    }
}
