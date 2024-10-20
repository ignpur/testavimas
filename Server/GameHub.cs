using Microsoft.AspNetCore.SignalR;
using Server.Strategy.ShipPlacement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class GameHub : Hub
    {
        private static Game CurrentGame = new Game();

        public async Task Join(string nickname, int seat, string placementStrategy)
        {
            IShipPlacementStrategy strategy;
            if (placementStrategy == "Random Placement")
            {
                strategy = new RandomShipPlacementStrategy();
            }
            else if (placementStrategy == "Mixed Placement")
            {
                strategy = new MixedShipPlacementStrategy();
            }
            else
            {
                strategy = new ManualShipPlacementStrategy();
            }
            var result = CurrentGame.JoinPlayer(nickname, seat, strategy);
            await Clients.Caller.SendAsync("JoinResult", result);


            if (CurrentGame.AreBothPlayersConnected())
            {
                CurrentGame.State = GameState.ArrangingShips;
                await Clients.All.SendAsync("GameState", "ArrangingShips");
                
            }
        }

        public async Task Disconnect(int player)
        {
            if (CurrentGame.DisconnectPlayer(player))
            {
                await Clients.All.SendAsync("GameState", "NotStarted");
            }
        }

        public async Task SetShip(int player, int size, int x, int y, bool vertical)
        {

            ShipHelper result = CurrentGame.SetShip(player, size, x, y, vertical);


            await Clients.Caller.SendAsync("ShipSet", result.result, result.posX, result.posY, result.size, result.vertical);
        }

        public async Task ReadyUp(int player)
        {
            var result = CurrentGame.ReadyUp(player);

            await Clients.All.SendAsync("PlayerReady", player, result, CurrentGame.Players[player].Name);
            

            if (CurrentGame.AreBothPlayersReady())
            {
                CurrentGame.Start();
                await Clients.All.SendAsync("GameState", "Started");
            }
        }

        public async Task Fire(int attacker, int x, int y)
        {
            var result = CurrentGame.Fire(attacker, x, y);

            await Clients.All.SendAsync("ShotFired", attacker, x, y, result);

            if (CurrentGame.CheckWin(attacker) == 1)
            {
                await Clients.All.SendAsync("GameWon", CurrentGame.Players[attacker].Name);
                return;
            }
        }

        public async Task ChatMessage(int seat, string message)
        {
            if (seat != 0 && seat != 1) return;

            var player = CurrentGame.Players[seat];
            if (player == null) return;

            await Clients.All.SendAsync("ChatMessage", player.Name, message);
        }
    }
}
