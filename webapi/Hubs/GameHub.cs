using Microsoft.AspNetCore.SignalR;
using webapi.Controllers.GameHub;
using webapi.Domain.PlayerConnectionDetails;
using webapi.Domain.PlayerDetails;
using webapi.Domain.Statics;

namespace webapi.Hubs
{
    public class GameHub : Hub
    {
        private readonly IGameHubController _gameHubController;

        public GameHub(IGameHubController gameHubController) {
            _gameHubController = gameHubController;
        }
    
        public async Task JoinGame(PlayerDetails playerDetails)
        {
            var playerConnectionDetails = new PlayerConnectionDetails(
                playerDetails.GameName,
                playerDetails.PlayerName,
                Context.ConnectionId
            );

            var errorMessage = await _gameHubController.JoinGame(playerConnectionDetails);

            if (errorMessage != null)
            {
                await Clients.Client(Context.ConnectionId).SendAsync("SendMessage", new { content = errorMessage, code = Enums.MessageCode.Error });
                Context.Abort();
            }
        }

        public async Task Bid(int bid, bool alone, string goingUnderCardsIds)
        {
            await _gameHubController.OnBid(Context.ConnectionId, bid, alone, goingUnderCardsIds);
        }

        public async Task DeclareTrump(int trumpSuitIndex, bool alone)
        {
            await _gameHubController.DeclareTrump(Context.ConnectionId, trumpSuitIndex, alone);
        }

        public async Task SwapPlayerPosition(string playerName)
        {
            await _gameHubController.SwapPlayerPosition(Context.ConnectionId, playerName);
        }

        public async Task DeclareReady(bool ready)
        {
            await _gameHubController.DeclareReady(Context.ConnectionId, ready);
        }

        public async Task CollectTrick(bool updateClients = true)
        {
            await _gameHubController.CollectTrick(Context.ConnectionId, updateClients);
        }

        public async Task PlayCard(int sentCardId)
        {
            await _gameHubController.PlayCard(Context.ConnectionId, sentCardId);
        }

        public async Task DiscardCard(int sentCardId)
        {
            await _gameHubController.DiscardCard(Context.ConnectionId, sentCardId);
        }

        public async Task ErrorMessage(string message)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("SendMessage", new { content = message, code = Enums.MessageCode.Error });
        }

        public async Task DoubleClickCard(int sentCardId)
        {
            await _gameHubController.DoubleClickCard(Context.ConnectionId, sentCardId);
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _gameHubController.OnClientDisconnected(Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
