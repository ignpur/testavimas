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

namespace Server
{
    public class GameHub : Hub
    {
        private static Game CurrentGame = new Game();
		//private static BoardConfiguration _sharedBoardConfiguration;
		private readonly GameFacade _gameFacade;

		public GameHub(GameFacade gameFacade)
		{
			_gameFacade = gameFacade;
		}

		public async Task Join(string nickname, int seat, string placementStrategy)
        {
			await _gameFacade.SetBoardChoiceAsync(Context.ConnectionId);
			await _gameFacade.JoinPlayerAsync(nickname, seat, placementStrategy);
		}

        public Task Disconnect(int player)
        {
			return _gameFacade.DisconnectPlayerAsync(player);
		}

        public Task SetShip(int player, int size, int x, int y, bool vertical)
        {

			return _gameFacade.SetShipAsync(player, size, x, y, vertical, Context.ConnectionId);
		}

        public Task ReadyUp(int player)
        {
			return _gameFacade.ReadyUpAsync(player);
		}

        public Task Fire(int attacker, int x, int y)
        {
			return _gameFacade.HandleFireAsync(attacker, x, y);

		}

        public Task ChatMessage(int seat, string message)
        {
			return _gameFacade.SendChatMessageAsync(seat, message);
		}
        public Task EnableSpecialPower(int seat, string powerType)
        {
			return _gameFacade.EnableSpecialPowerAsync(seat, powerType, Context.ConnectionId);
		}

        public Task Undo(int seat)
        {
			return _gameFacade.UndoAsync(seat, Context.ConnectionId);
		}
		public Task UpdateAsync(GameData gameData)
		{
			return _gameFacade.UpdateAsync(gameData);
		}

	}
}