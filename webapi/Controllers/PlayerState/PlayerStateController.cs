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

            var phase = game.GetPhase();
            var isPlaying = phase == Phase.Playing;

            player.SortHand(game.GetTrumpSuit());

            var playerIndex = player.GetIndex();
            var playerTurnIndex = game.GetPlayerTurnIndex();

            var isPlayersTurn = playerIndex == playerTurnIndex;

            var playerState = new HeroPlayerState
            {
                Name = playerName
            };

            var ignoredPlayerIndex = game.GetIgnoredPlayerIndex();
            if (!isPlaying || ignoredPlayerIndex == null || player.GetIndex() != ignoredPlayerIndex)
            {
                playerState.Hand.AddRange(player.GetHand());
            }

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

            var trumpSuit = game.GetTrumpSuit();

            var isInitializing = phase == Phase.Initializing;
            var isPickItUp = phase == Phase.Pick_It_Up;
            var isDeclaringTrump = phase == Phase.Declaring_Trump;
            var isRoundEnd = phase == Phase.RoundEnd;
            var isDiscard = phase == Phase.Discard;

            var showLastBid = isPickItUp || isDeclaringTrump;
            var highLightPlayer = showLastBid || isPlaying;
            var showReady = isInitializing || isRoundEnd;

            playerState.ShowSwapPosition = isInitializing;

            playerState.ShowTrumpIndicator = isPlaying || isDiscard;
            playerState.ShowTricksTaken = isPlaying;

            playerState.TeamOneTricksTaken = game.GetTricksTaken(0);
            playerState.TeamTwoTricksTaken = game.GetTricksTaken(1);

            playerState.ShowReady = showReady;
            playerState.IsReady = player.GetIsReady();

            playerState.TeamIndex = (playerIndex == 0 || playerIndex == 2) ? 0 : 1;

            playerState.ShowBiddingBox = isPickItUp && isPlayersTurn;
            playerState.Passed = player.GetPassed();

            if (isPickItUp)
            {
                playerState.TrumpCard = game.GetTrumpCard();
            }

            var currentTrick = _trickRepository.GetTrick(playerDetails);
            var collectCards = currentTrick != null && (currentTrick.GetCards().Count == 4 || (currentTrick.GetCards().Count == 3 && game.GetIgnoredPlayerIndex() != null));

            playerState.ShowPlayButton = isPlaying && isPlayersTurn && !collectCards;
            playerState.ShowCollectButton = isPlaying && isPlayersTurn && collectCards;

            playerState.ShowTrumpSelection = isDeclaringTrump && isPlayersTurn;

            var dealerIndex = game.GetDealerIndex();

            playerState.ShowDiscard = phase == Phase.Discard && isPlayersTurn;
            playerState.ShowGoingUnder = isPickItUp && player.CanGoUnder();
            playerState.ShowPickItUp = isPickItUp;
            playerState.ShowPassButton = isPickItUp || (isDeclaringTrump && player.GetIndex() != dealerIndex);
            playerState.ShowPassed = player.GetPassed() == true;

            var showDealer = isPickItUp || isDeclaringTrump || isDeclaringTrump || isDiscard;
            playerState.IsDealer = showDealer && playerIndex == dealerIndex;

            if (ally != null)
            {
                playerState.AllyState.Name = ally.GetName();

                playerState.AllyState.ShowReady = showReady;
                playerState.AllyState.IsReady = ally.GetIsReady();
                playerState.AllyState.Passed = ally.GetPassed();

                playerState.AllyState.IsDealer = showDealer && ally.GetIndex() == dealerIndex;

                playerState.AllyState.HighlightPlayer = highLightPlayer && playerTurnIndex == ally.GetIndex();
            }

            if (leftOpponent != null)
            {
                playerState.LeftOpponentState.Name = leftOpponent.GetName();

                playerState.LeftOpponentState.ShowReady = showReady;
                playerState.LeftOpponentState.IsReady = leftOpponent.GetIsReady();
                playerState.LeftOpponentState.Passed = leftOpponent.GetPassed();

                playerState.LeftOpponentState.IsDealer = showDealer && leftOpponent.GetIndex() == dealerIndex;

                playerState.LeftOpponentState.HighlightPlayer = highLightPlayer && playerTurnIndex == leftOpponent.GetIndex();
            }

            if (rightOpponent != null)
            {
                playerState.RightOpponentState.Name = rightOpponent.GetName();

                playerState.RightOpponentState.ShowReady = showReady;
                playerState.RightOpponentState.IsReady = rightOpponent.GetIsReady();
                playerState.RightOpponentState.Passed = rightOpponent.GetPassed();

                playerState.RightOpponentState.IsDealer = showDealer && rightOpponent.GetIndex() == dealerIndex;

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
