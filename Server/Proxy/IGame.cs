using Server.AbstractFactory;
using Server.Command;
using Server.Observer;
using Server.Strategy.ShipPlacement;
using System.Collections.Generic;

namespace Server.Proxy
{
    public interface IGame : ISubject
    {
        public GameState State { get; set; }
        public IPlayer[] Players { get; set; }
        public int Turn { get; set; }
        bool JoinPlayer(string name, int seat, IShipPlacementStrategy strategy);
        int setBoardChoice();
        bool DisconnectPlayer(int seat);
        bool ReadyUp(int player);
        ShipHelper SetShip(int player, int size, int x, int y, bool vertical);
        Shot Fire(int attacker, int x, int y);
        bool AreBothPlayersConnected();
        bool AreBothPlayersReady();
        int CheckWin(int player);
        ICommand CheckLastCommand(int player);
        void Undo(int player);
        void SavePlayerState(int player);
        List<List<bool>> RestorePlayerState(int player);
        public void Start();
        public void Initialize();
    }
}
