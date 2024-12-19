using System;

namespace Client
{
    public class GameJoinedEventArgs : EventArgs
    {
        public string GameId { get; }
        
        public GameJoinedEventArgs(string gameId)
        {
            GameId = gameId;
        }
    }
}