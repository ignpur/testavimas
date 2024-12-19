using System;
using Server.Bridge;

namespace Server.Adapter
{
    public class LoggerAdapter
    {
        private readonly Logger _logger;
        
        public LoggerAdapter(Logger logger)
        {
            _logger = logger;
        }
        
        public void LogFire(int playerId, int x, int y)
        {
            string message = $"Player {playerId} fired at {x}, {y}";
            _logger.Log("Fire", message);
        }
        public void LogShipPlacement(int playerId, int size, int x, int y, bool vertical)
        {
            string message = $"Player {playerId} placed a ship of size {size} at ({x}, {y})" + (vertical ? " vertically." : " horizontally.");
            _logger.Log("ShipPlacement", message);
        }
        public void LogJoin(string gameId, string nickname, int seat, string placementStrategy)
        {
            string message = $"Player {nickname} joined game {gameId} with seat {seat} using {placementStrategy} strategy.";
            _logger.Log("Join", message);
        }

        public void LogDisconnect(string gameId, int player)
        {
            string message = $"Player {player} disconnected from game {gameId}.";
            _logger.Log("Disconnect", message);
        }

        public void LogReadyUp(string gameId, int player)
        {
            string message = $"Player {player} is ready in game {gameId}.";
            _logger.Log("ReadyUp", message);
        }

        public void LogCreateGame(string gameId)
        {
            string message = $"Game {gameId} was created.";
            _logger.Log("CreateGame", message);
        }

        public void LogGetActiveGames(int activeGameCount)
        {
            string message = $"Active games were retrieved. Count {activeGameCount}";
            _logger.Log("GetActiveGames", message);
        }
    }
}