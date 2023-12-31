﻿using webapi.Controllers.Game;
using webapi.Controllers.Player;
using webapi.Controllers.PlayerConnection;
using webapi.Controllers.Trick;
using webapi.Domain.Game;
using webapi.Domain.GameDetails;
using webapi.Domain.Player;
using webapi.Domain.PlayerConnectionDetails;
using webapi.Domain.PlayerDetails;
using webapi.Domain.Statics;
using webapi.Domain.Tricks;
using static webapi.Domain.Statics.Enums;

namespace webapi.Controllers.GameHub
{
    public class GameHubController : IGameHubController
    {
        private readonly IGameController _gameController;
        private readonly IPlayerController _playerController;
        private readonly IPlayerConnectionController _playerConnectionController;
        private readonly ITrickController _trickController;

        public GameHubController(
            IGameController gameController,
            IPlayerController playerController,
            IPlayerConnectionController playerConnectionController,
            ITrickController trickController
        ) {
            _gameController = gameController;
            _playerController = playerController;
            _playerConnectionController = playerConnectionController;
            _trickController = trickController;
        }

        public async Task<string?> JoinGame(IPlayerConnectionDetails playerConnectionDetails)
        {
            return await _playerConnectionController.JoinGame(playerConnectionDetails);
        }

        public void OnClientDisconnected(string connectionId)
        {
            var playerDetails = _playerConnectionController.GetPlayerDetails(connectionId);

            if (playerDetails != null)
            {
                var connectedPlayerCount = _playerConnectionController.GetConnectedPlayerCount(playerDetails);

                if (connectedPlayerCount == 0)
                {
                    var game = _gameController.GetGame(playerDetails);
                    if (game != null)
                    {
                        var phase = game.GetPhase();

                        if (phase == Phase.Initializing)
                        {
                            _gameController.DeleteGame(playerDetails);
                        }
                    }
                }
            }

            _playerConnectionController.RemoveConnection(connectionId);
        }

        public async Task SwapPlayerPosition(string connectionId, string swapPlayerName)
        {
            var playerDetails = GetVerifiedPlayerDetails(connectionId);

            var game = GetVerifiedGame(playerDetails);

            ValidatePhase(playerDetails, game, Phase.Initializing);

            var players = _playerController.GetPlayers(playerDetails);

            var player = GetVerifiedPlayer(playerDetails);

            var swapPlayer = players.Where(player => player.GetName() == swapPlayerName).SingleOrDefault();
            if (swapPlayer == null)
            {
                await _playerConnectionController.MessageErrorToClient(playerDetails, "Swap player does not exist.");
                return;
            }

            _playerController.SwapPlayerIndices(player, swapPlayer);

            await _playerConnectionController.UpdateClients(playerDetails);
        }

        public async Task DeclareReady(string connectionId, bool ready)
        {
            var playerDetails = GetVerifiedPlayerDetails(connectionId);

            var players = _playerController.GetPlayers(playerDetails);

            var game = GetVerifiedGame(playerDetails);

            var player = players.Where(player => player.GetName() == playerDetails.GetPlayerName()).Single();
            if (player == null)
            {
                await _playerConnectionController.MessageErrorToClient(playerDetails, "Player does not exist.");
                return;
            }

            ValidatePhase(playerDetails, game, new List<Phase> { Phase.Initializing, Phase.RoundEnd });

            player.SetIsReady(ready);

            if (players.Count() == 4 && players.All(player => player.GetIsReady()))
            {
                var phase = game.GetPhase();
                if (phase == Phase.Initializing || phase == Phase.RoundEnd)
                {
                    Utils.StartNewRound(game, players);

                    var leadingPlayer = players.Single(player => player.GetIndex() == game.GetPlayerTurnIndex());
                    await _playerConnectionController.MessageClients(game.GetGameDetails(), $"The round has started! Bidding starts with {leadingPlayer.GetName()}.", leadingPlayer.GetTeamIndex());
                }
                else
                {
                    throw new InvalidOperationException("Game was in an unexpected phase.");
                }

                foreach (var pl in players)
                {
                    pl.SetIsReady(false);
                }
            }

            _gameController.UpdateGame(playerDetails, game);
            _playerController.UpdatePlayers(playerDetails, players);

            await _playerConnectionController.UpdateClients(playerDetails);
        }

        public async Task OnBid(string connectionId, int bid, bool alone, string goingUnderCardsIds)
        {
            var playerDetails = GetVerifiedPlayerDetails(connectionId);

            var playerName = playerDetails.GetPlayerName();

            var game = GetVerifiedGame(playerDetails);

            ValidatePhase(playerDetails, game, new List<Phase> { Phase.Pick_It_Up });

            var phase = game.GetPhase();

            var players = _playerController.GetPlayers(playerDetails);

            var biddingPlayer = players.Single(player => player.GetName() == playerName);

            var biddingPlayerIndex = biddingPlayer.GetIndex();
            var playerTurnIndex = game.GetPlayerTurnIndex();

            if (playerTurnIndex != biddingPlayerIndex)
            {
                await _playerConnectionController.MessageErrorToClient(playerDetails, "It is not that player's turn.");
                return;
            }

            var bidIsPass = bid < 0;
            var bidIsGoingUnder = bid == -2;
            var pickItUp = bid == 1;

            var teamIndex = biddingPlayer.GetTeamIndex();
            if (bidIsPass)
            {
                if (bidIsGoingUnder)
                {
                    await _playerConnectionController.MessageClients(playerDetails, $"{playerName} went under.", teamIndex);
                    biddingPlayer.GoUnder(game.GetAndThenReplenishKitty(goingUnderCardsIds), goingUnderCardsIds);
                }
                else
                {
                    await _playerConnectionController.MessageClients(playerDetails, $"{playerName} has passed!", teamIndex);
                }

                biddingPlayer.Pass();

                if (game.GetPlayerTurnIndex() == game.GetDealerIndex())
                {
                    if (phase == Phase.Pick_It_Up)
                    {
                        game.IncrementPhase();
                        game.FlipDealerCard();
                        _playerController.ResetBidState(playerDetails);
                    }
                }

                game.IncrementPlayerTurnIndex();
            }
            else if (pickItUp)
            {
                if (alone)
                {
                    game.LonerCalled(Utils.IncrementIndex(playerTurnIndex, 2));
                    await _playerConnectionController.MessageClients(playerDetails, $"{playerName} is going alone!", teamIndex);
                }
                else
                {
                    await _playerConnectionController.MessageClients(playerDetails, $"{playerName} says 'Pick it up'!", teamIndex);
                }

                var trumpSuit = game.GetTrumpCard()!.Suit;
                await _playerConnectionController.MessageClients(playerDetails, $"Trump is {trumpSuit}s!", teamIndex);

                game.SetTookBidTeamIndex(teamIndex);
                game.AddRoundBidResult(trumpSuit, teamIndex);

                game.SkipPhase();

                _playerController.ResetBidState(playerDetails);

                var dealerIndex = game.GetDealerIndex();
                var dealer = players.Single(player => player.GetIndex() == dealerIndex);

                game.SetPlayerTurnIndex(dealerIndex);
                dealer.DealCard(game.GetTrumpCard());

                _playerController.UpdatePlayers(playerDetails, players);
            }

            _gameController.UpdateGame(playerDetails, game);

            await _playerConnectionController.UpdateClients(playerDetails);
        }

        public async Task DeclareTrump(string connectionId, int trumpSuitIndex, bool alone)
        {
            var playerDetails = GetVerifiedPlayerDetails(connectionId);

            var game = GetVerifiedGame(playerDetails);

            var playerName = playerDetails.GetPlayerName();
            var playerTurnIndex = game.GetPlayerTurnIndex();

            ValidatePhase(playerDetails, game, Phase.Declaring_Trump);

            var players = _playerController.GetPlayers(playerDetails);
            var player = players.Single(player => player.GetName() == playerDetails.GetPlayerName());
            var playerIndex = player.GetIndex();

            if (playerIndex != playerTurnIndex)
            {
                await _playerConnectionController.MessageErrorToClient(playerDetails, "This player cannot declare trump.");
                return;
            }

            var teamIndex = Utils.GetTeamIndex(playerTurnIndex);

            if (trumpSuitIndex == -1)
            {
                if (playerIndex == game.GetDealerIndex())
                {
                    await _playerConnectionController.MessageErrorToClient(playerDetails, "Dealer cannot pass.");
                    return;
                }

                await _playerConnectionController.MessageClients(playerDetails, $"{playerName} has passed!", teamIndex);
                player.Pass();
                game.IncrementPlayerTurnIndex();
            }
            else
            {
                if (trumpSuitIndex < 0 || trumpSuitIndex > 3)
                {
                    await _playerConnectionController.MessageErrorToClient(playerDetails, "Invalid suit.");
                    return;
                }

                var trumpSuit = (Suit) trumpSuitIndex;

                game.SetTrumpSuit(trumpSuit);
                game.SkipPhase();

                _playerController.ResetBidState(playerDetails);

                await _playerConnectionController.MessageClients(playerDetails, $"Trump is {trumpSuit}s!", teamIndex);

                game.AddRoundBidResult(trumpSuit, teamIndex);
                game.SetTookBidTeamIndex(teamIndex);
                game.SetPlayerTurnIndex(Utils.IncrementIndex(game.GetDealerIndex()));
            }

            _gameController.UpdateGame(playerDetails, game);
            _playerController.UpdatePlayers(playerDetails, players);

            await _playerConnectionController.UpdateClients(playerDetails);
        }

        private IPlayerDetails GetVerifiedPlayerDetails(string connectionId)
        {
            return _playerConnectionController.GetPlayerDetails(connectionId)
                ?? throw new InvalidOperationException($"Player details were null for {connectionId}.");
        }

        private IGame GetVerifiedGame(IPlayerDetails playerDetails)
        {
            var game = _gameController.GetGame(playerDetails);
            if (game == null)
            {
                _playerConnectionController.MessageErrorToClient(playerDetails, "A game with that name does not exist.");
                throw new InvalidOperationException("A game with that name does not exist.");
            }
            else
            {
                return game;
            }
        }

        private IPlayer GetVerifiedPlayer(IPlayerDetails playerDetails)
        {
            var player = _playerController.GetPlayer(playerDetails);
            if (player == null)
            {
                _playerConnectionController.MessageErrorToClient(playerDetails, "A player with that name does not exist.");
                throw new InvalidOperationException("A game with that name does not exist.");
            }
            else
            {
                return player;
            }
        }

        private void ValidatePhase(IPlayerDetails playerDetails, IGame game, Phase phase)
        {
            var gamePhase = game.GetPhase();
            if (gamePhase != phase)
            {
                _playerConnectionController.MessageErrorToClient(playerDetails, "Invalid game phase.");
                throw new InvalidOperationException("Invalid phase.");
            }
        }

        private void ValidatePhase(IPlayerDetails playerDetails, IGame game, List<Phase> phases)
        {
            var gamePhase = game.GetPhase();
            if (!phases.Contains(gamePhase))
            {
                _playerConnectionController.MessageErrorToClient(playerDetails, "Invalid game phase.");
                throw new InvalidOperationException("Invalid phase.");
            }
        }

        public async Task PlayCard(string connectionId, int sentCardId)
        {
            var playerConnectionData = GetVerifiedPlayerDetails(connectionId);

            var game = GetVerifiedGame(playerConnectionData);

            var gameIsPlaying = game.GetPhase() == Phase.Playing;
            if (!gameIsPlaying)
            {
                await _playerConnectionController.MessageErrorToClient(playerConnectionData, "Game is not currently in a playing phase.");
                return;
            }

            var playerTurnIndex = game.GetPlayerTurnIndex();

            var player = GetVerifiedPlayer(playerConnectionData);
            var playerIndex = player.GetIndex();

            if (playerTurnIndex != playerIndex)
            {
                await _playerConnectionController.MessageErrorToClient(playerConnectionData, "Its not your turn.");
                return;
            }

            var hand = player.GetHand();

            var cardId = sentCardId;
            if (hand.Count != 1 && cardId == -1)
            {
                await _playerConnectionController.MessageErrorToClient(playerConnectionData, "You must select a card.");
                return;
            }
            else if (hand.Count == 1)
            {
                cardId = hand.Single().Id;
            }

            var card = hand.Where(card => card.Id == cardId).FirstOrDefault();
            if (card == null)
            {
                await _playerConnectionController.MessageErrorToClient(playerConnectionData, "You do not have that card to play.");
                return;
            }

            var currentTrick = _trickController.GetTrick(playerConnectionData);
            if (currentTrick != null && (currentTrick.GetCards().Count == 4 || (currentTrick.GetCards().Count == 3 && game.GetIgnoredPlayerIndex() != null)))
            {
                await CollectTrick(connectionId, false);
            }

            var trumpSuit = game.GetTrumpSuit();

            currentTrick = _trickController.GetTrick(playerConnectionData);
            if (currentTrick == null)
            {
                _trickController.CreateNewTrick(playerConnectionData, (Suit) trumpSuit!, card.Suit);

                currentTrick = _trickController.GetTrick(playerConnectionData) ?? throw new Exception("Created trick was null.");
            }
            else
            {
                int index = 0;
                var playedCards = currentTrick.GetCards().Select(card => new TrickCard(card.Id, card.Suit, card.Rank, index++)).ToList();

                var validPlays = Utils.GetValidPlays(playedCards, hand, (Suit) trumpSuit!);

                var canPlayCard = validPlays.Any(c => c.Suit == card.Suit && c.Rank == card.Rank);

                if (!canPlayCard)
                {
                    await _playerConnectionController.MessageErrorToClient(playerConnectionData, "You can't play that card.");
                    return;
                }
            }

            currentTrick.PlayCard(card, playerIndex);
            player.RemoveCard(card.Id);

            var trickPlays = currentTrick.GetTrickPlays();
            if (trickPlays.Count == 1)
            {
                var cardPronoun = card.Rank == Rank.Ace ? "an" : "a";
                var message = $"{player.GetName()} leads with {cardPronoun} {card.Rank} of {card.Suit}s!";

                game.IncrementPlayerTurnIndex();

                await _playerConnectionController.MessageClients(playerConnectionData, message, player.GetTeamIndex());
            }
            else if (trickPlays.Count == 4 || (trickPlays.Count == 3 && game.GetIgnoredPlayerIndex() != null))
            {
                int index = 0;
                var playedCards = currentTrick.GetCards().Select(card => new TrickCard(card.Id, card.Suit, card.Rank, index++)).ToList();

                var winningCardId = Utils.GetWinningCardId(currentTrick.GetTrumpSuit(), playedCards);

                var winningCard = Utils.GetCardFromId(winningCardId);

                var winningCardPronoun = winningCard.Rank == Rank.Ace ? "an" : "a";

                var winningPlayerIndex = trickPlays.Single(card => card.Card.Id == winningCardId).PlayerIndex;

                game.SetPlayerTurnIndex(winningPlayerIndex);

                var winningPlayer = _playerController.GetPlayers(playerConnectionData).Single(player => player.GetIndex() == winningPlayerIndex);

                var message = $"{winningPlayer.GetName()} has won the trick with {winningCardPronoun} {winningCard.Rank} of {winningCard.Suit}s!";

                await _playerConnectionController.MessageClients(playerConnectionData, message, winningPlayer.GetTeamIndex());
            }
            else
            {
                game.IncrementPlayerTurnIndex();
            }

            _gameController.UpdateGame(playerConnectionData, game);
            _playerController.UpdatePlayer(playerConnectionData, player);

            await _playerConnectionController.UpdateClients(playerConnectionData);
        }

        public async Task DoubleClickCard(string connectionId, int sentCardId)
        {
            var playerConnectionData = GetVerifiedPlayerDetails(connectionId);

            var game = GetVerifiedGame(playerConnectionData);

            if (game.GetPhase() == Phase.Playing)
            {
                await PlayCard(connectionId, sentCardId);
            }
            else if (game.GetPhase() == Phase.Discard)
            {
                await DiscardCard(connectionId, sentCardId);
            }
        }


        public async Task DiscardCard(string connectionId, int sentCardId)
        {
            var playerConnectionData = GetVerifiedPlayerDetails(connectionId);

            var game = GetVerifiedGame(playerConnectionData);

            var gameIsPlaying = game.GetPhase() == Phase.Discard;
            if (!gameIsPlaying)
            {
                await _playerConnectionController.MessageErrorToClient(playerConnectionData, "Game is not currently in a discarding phase.");
                return;
            }

            var playerTurnIndex = game.GetPlayerTurnIndex();

            var player = GetVerifiedPlayer(playerConnectionData);
            var playerIndex = player.GetIndex();

            if (playerTurnIndex != playerIndex)
            {
                await _playerConnectionController.MessageErrorToClient(playerConnectionData, "Its not your turn.");
                return;
            }

            var hand = player.GetHand();

            var card = hand.Where(card => card.Id == sentCardId).FirstOrDefault();
            if (card == null)
            {
                await _playerConnectionController.MessageErrorToClient(playerConnectionData, "You do not have that card to discard.");
                return;
            }

            player.RemoveCard(sentCardId);
            game.IncrementPhase();
            game.IncrementPlayerTurnIndex();

            _gameController.UpdateGame(playerConnectionData, game);
            _playerController.UpdatePlayer(playerConnectionData, player);

            await _playerConnectionController.UpdateClients(playerConnectionData);
        }


        public async Task CollectTrick(string connectionId, bool updateClients)
        {
            var playerConnectionData = GetVerifiedPlayerDetails(connectionId);

            var game = GetVerifiedGame(playerConnectionData);

            var currentTrick = _trickController.GetTrick(playerConnectionData);
            if (currentTrick == null)
            {
                await _playerConnectionController.MessageErrorToClient(playerConnectionData, "There's no trick to collect.");
                return;
            }

            var trickPlays = currentTrick.GetTrickPlays();
            if (trickPlays.Count != 4 || (trickPlays.Count < 3 && game.GetIgnoredPlayerIndex() != null))
            {
                await _playerConnectionController.MessageErrorToClient(playerConnectionData, "Trick is not complete.");
                return;
            }

            var playerTurnIndex = game.GetPlayerTurnIndex();

            var teamIndex = (playerTurnIndex == 0 || playerTurnIndex == 2) ? 0 : 1;
            game.AddCardIds(teamIndex, trickPlays.Select(play => play.Card.Id).ToList());

            var player = GetVerifiedPlayer(playerConnectionData);

            _trickController.DeleteTrick(playerConnectionData);

            if (player.GetHand().Count == 0)
            {
                game.IncrementPhase();
                var phase = game.GetPhase();

                ProcessRoundEnd(game);

                var tookBidTeamIndex = game.GetTookBidTeamIndex();

                var teamOneScore = game.GetTotalScore(0);
                var teamTwoScore = game.GetTotalScore(1);

                if (tookBidTeamIndex == 0 && teamOneScore >= 10)
                {
                    game.IncrementPhase();

                    var teamName = GetTeamName(playerConnectionData, 0);
                    await _playerConnectionController.MessageClients(playerConnectionData, $"{teamName} win!", tookBidTeamIndex);
                }
                else if (tookBidTeamIndex == 1 && teamTwoScore >= 10)
                {
                    game.IncrementPhase();

                    var teamName = GetTeamName(playerConnectionData, 1);
                    await _playerConnectionController.MessageClients(playerConnectionData, $"{teamName} win!", tookBidTeamIndex);
                }
                else if (teamOneScore < 0 && teamTwoScore >= 10)
                {
                    game.IncrementPhase();

                    var teamName = GetTeamName(playerConnectionData, 1);
                    await _playerConnectionController.MessageClients(playerConnectionData, $"{teamName} win!", tookBidTeamIndex);
                }
                else if (teamTwoScore < 0 && teamOneScore >= 10)
                {
                    game.IncrementPhase();

                    var teamName = GetTeamName(playerConnectionData, 0);
                    await _playerConnectionController.MessageClients(playerConnectionData, $"{teamName} win!", tookBidTeamIndex);
                }
            }

            _gameController.UpdateGame(playerConnectionData, game);

            if (updateClients)
            {
                await _playerConnectionController.UpdateClients(playerConnectionData);
            }
        }

        private string GetTeamName(IGameDetails gameDetails, int index)
        {
            var playerNames = _playerController.GetPlayers(gameDetails).OrderBy(player => player.GetIndex()).Select(player => player.GetName()).ToList();

            return index == 0
                ? $"{playerNames.ElementAtOrDefault(0) ?? ""} and {playerNames.ElementAtOrDefault(2) ?? ""}"
                : $"{playerNames.ElementAtOrDefault(1) ?? ""} and {playerNames.ElementAtOrDefault(3) ?? ""}";
        }

        private void ProcessRoundEnd(IGame game)
        {
            var gameDetails = game.GetGameDetails();

            var tookBidTeamIndex = game.GetTookBidTeamIndex();
            var otherTeamIndex = game.GetTookBidTeamIndex() == 0 ? 1 : 0;

            var tookBidTeamName = GetTeamName(gameDetails, tookBidTeamIndex);
            var tricksTakenByWinningteam = game.GetTricksTaken(tookBidTeamIndex);
            
            if (tricksTakenByWinningteam == 5)
            {
                var scoreAdded = game.TeamWasAlone(tookBidTeamIndex) ? 4 : 2;

                game.AddScore(tookBidTeamIndex, scoreAdded);
                game.AddScore(otherTeamIndex, 0);

                _playerConnectionController.MessageClients(gameDetails, $"{tookBidTeamName} took {tricksTakenByWinningteam} tricks and score {scoreAdded} points!", tookBidTeamIndex);
            }
            else if (tricksTakenByWinningteam > 2)
            {
                game.AddScore(tookBidTeamIndex, 1);
                game.AddScore(otherTeamIndex, 0);
                _playerConnectionController.MessageClients(gameDetails, $"{tookBidTeamName} took {tricksTakenByWinningteam} tricks and score 1 point!", tookBidTeamIndex);
            }
            else
            {
                var otherTeamName = GetTeamName(gameDetails, otherTeamIndex);
                _playerConnectionController.MessageClients(gameDetails, $"{tookBidTeamName} took only {tricksTakenByWinningteam} tricks, so {otherTeamName} score 2 points.", otherTeamIndex);

                game.AddScore(otherTeamIndex, 2);
                game.AddScore(tookBidTeamIndex, 0);
            }

        }
    }
}
