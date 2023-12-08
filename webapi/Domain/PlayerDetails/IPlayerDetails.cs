using webapi.Domain.GameDetails;

namespace webapi.Domain.PlayerDetails
{
    public interface IPlayerDetails : IGameDetails
    {
        string GetPlayerName();
    }
}
