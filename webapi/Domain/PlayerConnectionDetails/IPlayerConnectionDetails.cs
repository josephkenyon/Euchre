using webapi.Domain.PlayerDetails;

namespace webapi.Domain.PlayerConnectionDetails
{
    public interface IPlayerConnectionDetails : IPlayerDetails
    {
        string GetConnectionId();
    }
}

