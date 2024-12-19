using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Server.Facade;
using Server.Proxy;

namespace Server
{
    public class GameManager
    {
        private static readonly object Lock = new();
        private static GameManager _instance;
        private readonly Dictionary<string, GameFacade> _gameFacades = new();
        private readonly IHubContext<GameHub> _hubContext;

        private GameManager(IHubContext<GameHub> hubContext)
        {
            _hubContext = hubContext;
        }
        
        public static GameManager Instance(IHubContext<GameHub> hubContext)
        {
                lock (Lock)
                {
                    if (_instance == null)
                    {
                        _instance = new GameManager(hubContext);
                    }
                }
                return _instance;
        }
        
        public GameFacade GetOrCreateGameFacade(string gameId)
        {
            lock (Lock)
            {
                if (_gameFacades.ContainsKey(gameId)) return _gameFacades[gameId];
                //var game = new Game();
                var game = new GameProxy();
                var gameFacade = new GameFacade(game, _hubContext);
                _gameFacades[gameId] = gameFacade;
                return _gameFacades[gameId];
            }
        }
        
        public bool RemoveGameFacade(string gameId)
        {
            lock (Lock)
            {
                return _gameFacades.Remove(gameId);
            }
        }
        
        public IEnumerable<string> GetActiveGamesFacades()
        {
            lock (Lock)
            {
                return _gameFacades.Keys;
            }
        }
    }
}