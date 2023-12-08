namespace webapi.Domain.PlayerDetails
{
    public class PlayerDetails : GameDetails.GameDetails, IPlayerDetails
    {
        public string PlayerName { get; set; }

        public PlayerDetails(string gameName, string playerName) : base(gameName)
        {
            PlayerName = playerName;
        }

        public string GetPlayerName()
        {
            return PlayerName;
        }
    }
}
