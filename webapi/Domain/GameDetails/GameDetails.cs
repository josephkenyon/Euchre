namespace webapi.Domain.GameDetails
{
    public class GameDetails : IGameDetails
    {
        public string GameName { get; set; }

        public GameDetails(string gameName)
        {
            GameName = gameName;
        }

        public string GetGameName()
        {
            return GameName;
        }
    }
}
