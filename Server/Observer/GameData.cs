using System.Collections.Generic;

namespace Server.Observer
{
    public class GameData
    {
        public string CurrentPlayer { get; set; }
        public string StatusMessage { get; set; }
        public List<Move> RecentMoves { get; set; } // Add a list of recent moves

        public GameData()
        {
            RecentMoves = new List<Move>();
        }
    }

    public class Move
    {
        public int PlayerSeat { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool Hit { get; set; }
    }
}
