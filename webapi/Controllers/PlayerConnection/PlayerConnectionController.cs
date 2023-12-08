
using Microsoft.AspNetCore.SignalR;
using webapi.Controllers.Game;
using webapi.Controllers.Player;
using webapi.Controllers.PlayerState;
using webapi.Domain.GameDetails;
using webapi.Domain.PlayerConnectionDetails;
using webapi.Domain.PlayerDetails;
using webapi.Repositories.PlayerConnection;
using static webapi.Domain.Statics.Enums;

namespace webapi.Controllers.PlayerConnection
{
    public class PlayerConnectionController : IPlayerConnectionController
    {
        private readonly ILogger<PlayerConnectionController> _logger;
        private readonly IPlayerStateController _playerStateController;
        private readonly IPlayerConnectionRepository _playerConnectionRepository;
        private readonly IGameController _gameController;
        private readonly IPlayerController _playerController;
        private readonly IHubContext<Hubs.GameHub> _hubContext;

        public PlayerConnectionController(
            ILogger<PlayerConnectionController> logger,
            IPlayerStateController playerStateController,
            IPlayerConnectionRepository playerConnectionRepository,
            IGameController gameController,
            IPlayerController playerController,
            IHubContext<Hubs.GameHub> hubContext
        )
        {
            _logger = logger;
            _playerStateController = playerStateController;
            _playerConnectionRepository = playerConnectionRepository;
            _gameController = gameController;
            _playerController = playerController;
            _hubContext = hubContext;
        }

        public async Task<string?> JoinGame(IPlayerConnectionDetails playerConnectionDetails)
        {
            var gameName = playerConnectionDetails.GetGameName();
            if (string.IsNullOrWhiteSpace(gameName))
            {
                return "Please enter a game name.";
            }

            var playerName = playerConnectionDetails.GetPlayerName();
            if (string.IsNullOrWhiteSpace(playerName))
            {
                return "Please enter a player name.";
            }

            var existingClient = _playerConnectionRepository.GetPlayerConnection(playerConnectionDetails);
            if (existingClient != null)
            {
                return "Someone is already playing in that game under than name.";
            }

            var game = _gameController.GetGame(playerConnectionDetails);

            var gameIsInitializing = game != null && game.GetPhase() == Phase.Initializing;

            var existingPlayer = _playerController.GetPlayer(playerConnectionDetails);

            if (game == null)
            {
                _gameController.CreateGame(playerConnectionDetails);
                _playerController.CreatePlayer(playerConnectionDetails);
            }
            else if (existingPlayer == null)
            {
                var gameIsFull = _playerController.GetPlayers(playerConnectionDetails).Count() == 4;
                if (gameIsInitializing && !gameIsFull)
                {
                    _playerController.CreatePlayer(playerConnectionDetails);
                }
                else
                {
                    return gameIsFull ? "That game is full." : "You cannot add a new player to a game already in progress.";
                }
            }

            _playerConnectionRepository.CreateConnection(playerConnectionDetails);

            await UpdateClients(playerConnectionDetails);

            return null;
        }

        public int GetConnectedPlayerCount(IGameDetails gameDetails)
        {
            var connections = _playerConnectionRepository.GetPlayerConnections(gameDetails);

            return connections.Count();
        }

        public void RemoveConnection(string connectionId)
        {
            var playerDetails = GetPlayerDetails(connectionId);

            if (playerDetails != null)
            {
                _playerConnectionRepository.DeleteConnection(connectionId);
            }
        }

        public IPlayerDetails? GetPlayerDetails(string connectionId)
        {
            return _playerConnectionRepository.GetPlayerDetails(connectionId);
        }

        private IPlayerConnectionDetails GetPlayerConnectionDetails(IPlayerDetails playerDetails)
        {
            var playerConnectionDetails = _playerConnectionRepository.GetPlayerConnection(playerDetails);

            return playerConnectionDetails ?? throw new InvalidOperationException($"Player details were null for {playerDetails.GetPlayerName()}");
        }

        public async Task UpdateClients(IGameDetails gameDetails)
        {
            foreach (var connection in _playerConnectionRepository.GetPlayerConnections(gameDetails))
            {
                try
                {
                    var playerState = _playerStateController.GetPlayerState(connection);
                    await _hubContext.Clients.Client(connection.GetConnectionId()).SendAsync("UpdatePlayerState", playerState);
                }
                catch (Exception ex)
                {
                    _logger.LogError(default, ex, "Error occurred while updating clients.");
                }
            }
        }

        public async Task MessageClients(IGameDetails gameDetails, string content, int teamIndex)
        {
            var code = teamIndex == 0 ? MessageCode.TeamOne : MessageCode.TeamTwo;

            await MessageClients(gameDetails, content, code);   
        }

        private async Task MessageClients(IGameDetails gameDetails, string content, MessageCode code)
        {
            foreach (var connection in _playerConnectionRepository.GetPlayerConnections(gameDetails))
            {
                await MessageClient(connection, code, content);
            }
        }

        public async Task MessageClient(IPlayerDetails playerDetails, string content, int teamIndex)
        {
            var code = teamIndex == 0 ? MessageCode.TeamOne : MessageCode.TeamTwo;

            var playerConnectionDetails = GetPlayerConnectionDetails(playerDetails);

            await MessageClient(playerConnectionDetails, code, content);
        }

        private async Task MessageClient(IPlayerConnectionDetails playerConnectionDetails, MessageCode code, string content)
        {
            try
            {
                await _hubContext.Clients.Client(playerConnectionDetails.GetConnectionId()).SendAsync("SendMessage", new { content, code });
            }
            catch (Exception ex)
            {
                _logger.LogError(default, ex, "Error occurred while messaging a client.");
            }
        }

        public async Task MessageError(IPlayerDetails playerDetails, string errorMessage)
        {
            var playerConnectionDetails = GetPlayerConnectionDetails(playerDetails);

            await MessageClient(playerConnectionDetails, MessageCode.Error, errorMessage);
        }

        public async Task MessageErrorToClients(IGameDetails gameDetails, string errorMessage)
        {
            await MessageClients(gameDetails, errorMessage, MessageCode.Error);
        }

        public async Task MessageErrorToClient(IPlayerDetails playerDetails, string errorMessage)
        {
            var playerConnectionDetails = GetPlayerConnectionDetails(playerDetails);

            await MessageClient(playerConnectionDetails, MessageCode.Error, errorMessage);
        }
    }
}
