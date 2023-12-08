namespace webapi.Domain.Statics
{
    public static class Enums
    {
        public enum Suit
        {
            Spade,
            Heart,
            Club,
            Diamond
        }

        public enum Rank
        {
            Ace,
            King,
            Queen,
            Jack,
            Ten,
            Nine
        }

        public enum Phase
        {
            Initializing,
            Pick_It_Up,
            Declaring_Trump,
            Discard,
            Playing,
            RoundEnd
        }

        public enum MessageCode
        {
            TeamOne,
            TeamTwo,
            Error
        }
    }
}
