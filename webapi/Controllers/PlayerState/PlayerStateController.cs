using webapi.Domain.PlayerDetails;
using webapi.Domain.PlayerState;
using webapi.Domain.Statics;
using webapi.Repositories.Game;
using webapi.Repositories.Player;
using webapi.Repositories.Trick;
using static webapi.Domain.Statics.Enums;

namespace webapi.Controllers.PlayerState
{
    public class PlayerStateController : IPlayerStateController
    {
        private readonly IGameRepository _gameRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly ITrickRepository _trickRepository;

        public PlayerStateController(
            IGameRepository gameRepository,
            IPlayerRepository playerRepository,
            ITrickRepository trickRepository
        ) {
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
            _trickRepository = trickRepository;
        }

        public HeroPlayerState GetPlayerState(IPlayerDetails playerDetails)
        {
            var gameName = playerDetails.GetGameName();
            var playerName = playerDetails.GetPlayerName();

            var game = _gameRepository.GetGame(gameName) ?? throw new Exception();
            var player = _playerRepository.GetPlayer(playerDetails) ?? throw new Exception();

            var playerIndex = player.GetIndex();
            var playerTurnIndex = game.GetPlayerTurnIndex();

            var isPlayersTurn = playerIndex == playerTurnIndex;

            var playerState = new HeroPlayerState
            {
                Name = playerName
            };

            playerState.Hand.AddRange(player.GetHand());

            var allyIndex = playerIndex + 2;
            if (allyIndex > 3)
            {
                allyIndex -= 4;
            }

            var players = _playerRepository.GetPlayers(playerDetails);
            var ally = players.SingleOrDefault(player => player.GetIndex() == allyIndex);

            var leftOpponentIndex = playerIndex + 1;
            if (leftOpponentIndex > 3)
            {
                leftOpponentIndex -= 4;
            }
            var leftOpponent = players.Where(player => player.GetIndex() == leftOpponentIndex).SingleOrDefault();

            var rightOpponentIndex = playerIndex + 3;
            if (rightOpponentIndex > 3)
            {
                rightOpponentIndex -= 4;
            }
            var rightOpponent = players.SingleOrDefault(player => player.GetIndex() == rightOpponentIndex);

            var phase = game.GetPhase();
            var trumpSuit = game.GetTrumpSuit();

            var isInitializing = phase == Phase.Initializing;
            var isBidding = phase == Phase.Pick_It_Up;
            var isPlaying = phase == Phase.Playing;
            var isDeclaringTrump = phase == Phase.Declaring_Trump;
            var isRoundEnd = phase == Phase.RoundEnd;

            var showLastBid = isBidding || isDeclaringTrump;
            var highLightPlayer = showLastBid || isPlaying;
            var showReady = isInitializing || isRoundEnd;

            playerState.ShowSwapPosition = isInitializing;

            playerState.ShowTrumpIndicator = isPlaying;
            playerState.ShowTricksTaken = isPlaying;

            playerState.TeamOneTricksTaken = game.GetCardIds(0).Count / 4;
            playerState.TeamTwoTricksTaken = game.GetCardIds(1).Count / 4;

            playerState.ShowReady = showReady;
            playerState.IsReady = player.GetIsReady();

            playerState.TeamIndex = (playerIndex == 0 || playerIndex == 2) ? 0 : 1;

            playerState.ShowBiddingBox = isBidding && isPlayersTurn;
            playerState.CurrentBid = game.GetCurrentBid();
            playerState.LastBid = player.GetLastBid();
            playerState.ShowLastBid = showLastBid && playerState.LastBid != 0;

            var currentTrick = _trickRepository.GetTrick(playerDetails);
            var collectCards = currentTrick != null && currentTrick.GetCards().Count == 4;

            playerState.ShowPlayButton = isPlaying && isPlayersTurn && !collectCards;
            playerState.ShowCollectButton = isPlaying && isPlayersTurn && collectCards;

            playerState.ShowTrumpSelection = isDeclaringTrump && isPlayersTurn;

            if (ally != null)
            {
                playerState.AllyState.Name = ally.GetName();

                playerState.AllyState.ShowReady = showReady;
                playerState.AllyState.IsReady = ally.GetIsReady();

                var lastBid = ally.GetLastBid();
                playerState.AllyState.ShowLastBid = showLastBid && lastBid != 0;
                playerState.AllyState.LastBid = lastBid;

                playerState.AllyState.HighlightPlayer = highLightPlayer && playerTurnIndex == ally.GetIndex();
            }

            if (leftOpponent != null)
            {
                playerState.LeftOpponentState.Name = leftOpponent.GetName();

                playerState.LeftOpponentState.ShowReady = showReady;
                playerState.LeftOpponentState.IsReady = leftOpponent.GetIsReady();

                var lastBid = leftOpponent.GetLastBid();
                playerState.LeftOpponentState.ShowLastBid = showLastBid && lastBid != 0;
                playerState.LeftOpponentState.LastBid = lastBid;

                playerState.LeftOpponentState.HighlightPlayer = highLightPlayer && playerTurnIndex == leftOpponent.GetIndex();
            }

            if (rightOpponent != null)
            {
                playerState.RightOpponentState.Name = rightOpponent.GetName();

                playerState.RightOpponentState.ShowReady = showReady;
                playerState.RightOpponentState.IsReady = rightOpponent.GetIsReady();

                var lastBid = rightOpponent.GetLastBid();
                playerState.RightOpponentState.ShowLastBid = showLastBid && lastBid != 0;
                playerState.RightOpponentState.LastBid = lastBid;

                playerState.RightOpponentState.HighlightPlayer = highLightPlayer && playerTurnIndex == rightOpponent.GetIndex();
            }

            playerState.HighlightPlayer = highLightPlayer && playerTurnIndex == playerIndex;

            var playerNames = players.OrderBy(player => player.GetIndex()).Select(player => player.GetName()).ToList();

            var teamOneName = $"{playerNames.ElementAtOrDefault(0) ?? ""}/{playerNames.ElementAtOrDefault(2) ?? ""}";
            var teamTwoName = $"{playerNames.ElementAtOrDefault(1) ?? ""}/{playerNames.ElementAtOrDefault(3) ?? ""}";

            if (currentTrick != null)
            {
                playerState.TrickState = Utils.GetTrickState(currentTrick, playerIndex);
            }

            playerState.TeamOneScoreList = game.GetScoreLog(teamOneName, 0);
            playerState.TeamTwoScoreList = game.GetScoreLog(teamTwoName, 1);
            playerState.RoundBidResults = game.GetRoundBidResults();

            return playerState;
        }
    }
}
