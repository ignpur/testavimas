using Microsoft.AspNetCore.SignalR;
using Server.Command;
using Server.Decorator;
using Server.Observer;
using Server.Prototype;
using Server.Proxy;
using Server.Strategy.ShipPlacement;
using Server.TemplateMethod;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Facade
{
	public class GameFacade : IObserver
	{
		private readonly IGame _game;
		private readonly IHubContext<GameHub> _hubContext;

		public GameFacade(IGame game, IHubContext<GameHub> hubContext)
		{
			_game = game;
			_hubContext = hubContext;
		}

		// Sets board choice and sends it to the calling client
		public async Task<int> SetBoardChoiceAsync(string connectionId)
		{
			int choice = _game.setBoardChoice();
			await _hubContext.Clients.Client(connectionId).SendAsync("SendBoardChoice", choice);
			return choice;
		}

		// Handles player joining the game with a specified strategy
		public async Task<bool> JoinPlayerAsync(string name, int seat, string placementStrategy, string gameId)
		{
			IShipPlacementStrategy strategy = CreateStrategy(placementStrategy);
			bool result = _game.JoinPlayer(name, seat, strategy);

			if (result && _game.AreBothPlayersConnected())
			{
				_game.State = GameState.ArrangingShips;
				await _hubContext.Clients.Group(gameId).SendAsync("GameState", "ArrangingShips");
			}

			return result;
		}

		// Configures and returns the appropriate placement strategy
		private IShipPlacementStrategy CreateStrategy(string placementStrategy)
		{
			return placementStrategy switch
			{
				"Random Placement" => new RandomShipPlacementStrategy(),
				"Mixed Placement" => new MixedShipPlacementStrategy(),
				_ => new ManualShipPlacementStrategy()
			};
		}
		// Manages player disconnection
		public async Task DisconnectPlayerAsync(int seat, string gameId)
		{
			bool result = _game.DisconnectPlayer(seat);
			if (result)
			{
				_game.Detach(this);
				await _hubContext.Clients.Group(gameId).SendAsync("GameState", "NotStarted");
			}
		}

		// Handles setting a ship on the board
		public async Task SetShipAsync(int player, int size, int x, int y, bool vertical, string connectionId)
		{
			ShipHelper result = _game.SetShip(player, size, x, y, vertical);
			await _hubContext.Clients.Client(connectionId).SendAsync("ShipSet", result.result, result.posX, result.posY, result.size, result.vertical);
		}
		// Adds functionality to ready up the game
		public async Task<bool> ReadyUpAsync(int seat, string gameId)
		{
			bool isReady = _game.ReadyUp(seat);
			await _hubContext.Clients.Group(gameId).SendAsync("PlayerReady", seat, isReady, _game.Players[seat].getName());

			if (_game.AreBothPlayersReady())
			{
				_game.Start();
				_game.Attach(this);
				await _hubContext.Clients.Group(gameId).SendAsync("GameState", "Started");
			}

			return isReady;
		}
		public async Task HandleFireAsync(int attacker, int x, int y, string gameId)
		{
            PlayerTurnTemplate playerTurn = (attacker == 0)
			? new PlayerOneTurn(_game, x, y)
			: new PlayerTwoTurn(_game, x, y);

            playerTurn.ExecuteTurn();

            var shot = _game.Fire(attacker, x, y);

			// Notify all clients about the shot
			await _hubContext.Clients.Group(gameId).SendAsync("ShotFired", attacker, shot.x, shot.y, shot.result);

			// Check if extra shot is available and notify the attacker
			if (!shot.result && _game.Players[attacker] is ExtraShotDecorator extraShotPlayer && extraShotPlayer.IsExtraShotAvailable())
			{
				Console.WriteLine($"Player {attacker} has an extra shot!");
				extraShotPlayer.ResetExtraShot();
				await _hubContext.Clients.Group(gameId).SendAsync("ExtraShotAllowed");
			}

			// Check for win condition
			if (_game.CheckWin(attacker) == 1)
			{
				_game.Detach(this);
				await _hubContext.Clients.Group(gameId).SendAsync("GameWon", _game.Players[attacker].getName());
			}
		}
		// Simplifies firing a shot

		// Simplifies power-up activation
		public async Task EnableSpecialPowerAsync(int seat, string powerType, string connectionId)
		{
			if (_game.Players[seat] != null)
			{
				if (powerType == "Extra Shot")
				{
					_game.Players[seat] = new ExtraShotDecorator(_game.Players[seat]);
				}
				else if (powerType == "Auto Dodge")
				{
					_game.Players[seat] = new AutoDodgeDecorator(_game.Players[seat]);
				}
				await _hubContext.Clients.Client(connectionId).SendAsync("SpecialPowerEnabled", powerType);
			}
		}
		// Handles chat messages between players
		public async Task SendChatMessageAsync(int seat, string message, string gameId)
		{
			var player = _game.Players[seat];
			if (player == null) return;

			await _hubContext.Clients.Group(gameId).SendAsync("ChatMessage", player.getName(), message);
		}
		// Handles undo action for a player
		public async Task UndoAsync(int seat, string connectionId)
		{
			var player = _game.Players[seat];
			if (player == null) return;

			var lastCommand = _game.CheckLastCommand(seat);
			_game.Undo(seat);

			switch (lastCommand)
			{
				case PlaceShipCommand placeShipCommand:
					ShipHelper shipResults = placeShipCommand.GetResults();
					await _hubContext.Clients.Client(connectionId).SendAsync("UnmarkShip", shipResults.posX, shipResults.posY, shipResults.size, shipResults.vertical);
					break;
				default:
					Console.WriteLine("Unknown command type or no action to undo.");
					break;
			}
		}
		public Task UpdateAsync(GameData gameData, string gameId)
		{
			Console.WriteLine(gameData.StatusMessage);
			return _hubContext.Clients.Group(gameId).SendAsync("ObserverMessage", gameData.StatusMessage);
		}

		public void Update(GameData gameData)
		{
			Console.WriteLine(gameData.StatusMessage);
		}

        public async Task SavePlayerState(int player, string connectionId)
        {
            _game.SavePlayerState(player);
            await _hubContext.Clients.Client(connectionId).SendAsync("StateSaved");
        }

        public async Task RestorePlayerState(int player, string connectionId)
        {
			var board = _game.RestorePlayerState(player);
			Console.WriteLine("Board reset");
            await _hubContext.Clients.Client(connectionId).SendAsync("BoardStateUpdated", board);
        }
    }
}
