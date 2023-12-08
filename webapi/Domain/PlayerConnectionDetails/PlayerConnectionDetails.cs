namespace webapi.Domain.PlayerConnectionDetails
{
    public class PlayerConnectionDetails : PlayerDetails.PlayerDetails, IPlayerConnectionDetails
    {
        public string Id { get; set; }

        public PlayerConnectionDetails(string gameName, string playerName, string id) : base(gameName, playerName)
        {
            Id = id;
        }

        public string GetConnectionId()
        {
            return Id;
        }
    }
}

