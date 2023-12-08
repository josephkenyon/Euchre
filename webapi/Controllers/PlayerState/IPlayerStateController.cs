using webapi.Domain.PlayerDetails;
using webapi.Domain.PlayerState;

namespace webapi.Controllers.PlayerState
{
    public interface IPlayerStateController
    {
        HeroPlayerState GetPlayerState(IPlayerDetails playerDetails);

    }
}
