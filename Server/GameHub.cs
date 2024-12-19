using Microsoft.AspNetCore.SignalR;
using Server.Strategy.ShipPlacement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Server.Decorator;
using Server.Command;
using Server.Facade;
using Server.Observer;
using System.Xml.Linq;
using Server.Adapter;
using Server.Prototype;

namespace Server
{
    public class GameHub : Hub
    {
        private static Game CurrentGame = new Game();
		//private static BoardConfiguration _sharedBoardConfiguration;
		//private readonly GameFacade _gameFacade;
		private readonly GameManager _gameManager;
		
		private readonly LoggerAdapter _playerActionLoggerAdapter;
		private readonly LoggerAdapter _gameEventLoggerAdapter;

		public GameHub(GameFacade gameFacade, IHubContext<GameHub> hubContext, LoggerAdapter playerActionLoggerAdapter, LoggerAdapter gameEventLoggerAdapter)
		{
			//_gameFacade = gameFacade;
			_gameManager = GameManager.Instance(hubContext);
			_playerActionLoggerAdapter = playerActionLoggerAdapter;
			_gameEventLoggerAdapter = gameEventLoggerAdapter;
		}


		public async Task Join(string gameId, string nickname, int seat, string placementStrategy)
        {
	        var gameFacade = _gameManager.GetOrCreateGameFacade(gameId);
	        await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
			await gameFacade.SetBoardChoiceAsync(Context.ConnectionId);
			await gameFacade.JoinPlayerAsync(nickname, seat, placementStrategy, gameId);
			_playerActionLoggerAdapter.LogJoin(gameId, nickname, seat, placementStrategy);
		}

        public Task Disconnect(string gameId, int player)
        {
	        var gameFacade = _gameManager.GetOrCreateGameFacade(gameId);
	        Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId);
	        _playerActionLoggerAdapter.LogDisconnect(gameId, player);
			return gameFacade.DisconnectPlayerAsync(player, gameId);
		}

        public Task SetShip(string gameId, int player, int size, int x, int y, bool vertical)
        {
	        var gameFacade = _gameManager.GetOrCreateGameFacade(gameId);
	        _playerActionLoggerAdapter.LogShipPlacement(player, size, x, y, vertical);
			return gameFacade.SetShipAsync(player, size, x, y, vertical, Context.ConnectionId);
		}

        public Task ReadyUp(string gameId, int player)
        {
	        var gameFacade = _gameManager.GetOrCreateGameFacade(gameId);
	        _playerActionLoggerAdapter.LogReadyUp(gameId, player);
			return gameFacade.ReadyUpAsync(player, gameId);
		}

        public Task Fire(string gameId, int attacker, int x, int y)
        {
	        var gameFacade = _gameManager.GetOrCreateGameFacade(gameId);
	        _playerActionLoggerAdapter.LogFire(attacker, x, y);
			return gameFacade.HandleFireAsync(attacker, x, y, gameId);
		}

        public Task ChatMessage(string gameId, int seat, string message)
        {
	        var gameFacade = _gameManager.GetOrCreateGameFacade(gameId);
			return gameFacade.SendChatMessageAsync(seat, message, gameId);
		}
        public Task EnableSpecialPower(string gameId, int seat, string powerType)
        {
	        var gameFacade = _gameManager.GetOrCreateGameFacade(gameId);
			return gameFacade.EnableSpecialPowerAsync(seat, powerType, Context.ConnectionId);
		}

        public Task Undo(string gameId, int seat)
        {
	        var gameFacade = _gameManager.GetOrCreateGameFacade(gameId);
			return gameFacade.UndoAsync(seat, Context.ConnectionId);
		}
		public Task UpdateAsync(string gameId, GameData gameData)
		{
			var gameFacade = _gameManager.GetOrCreateGameFacade(gameId);
			return gameFacade.UpdateAsync(gameData, gameId);
		}

		public Task<List<string>> GetActiveGames()
		{
			var activeGames = _gameManager.GetActiveGamesFacades();
			var enumerable = activeGames.ToList();
			_gameEventLoggerAdapter.LogGetActiveGames(enumerable.Count());
			return Task.FromResult(enumerable);
		}

		public async Task<string> CreateGame() 
		{
			var gameId = Guid.NewGuid().ToString();
			_gameManager.GetOrCreateGameFacade(gameId);
			_gameEventLoggerAdapter.LogCreateGame(gameId);
			await Clients.All.SendAsync("GameCreated", gameId);
			return gameId;
		}

        public Task SavePlayerState(string gameId, int player)
        {
            var gameFacade = _gameManager.GetOrCreateGameFacade(gameId);
			return gameFacade.SavePlayerState(player, Context.ConnectionId);
        }

        public Task RestorePlayerState(string gameId, int player)
        {
            var gameFacade = _gameManager.GetOrCreateGameFacade(gameId);
            return gameFacade.RestorePlayerState(player, Context.ConnectionId);
        }
    }
}