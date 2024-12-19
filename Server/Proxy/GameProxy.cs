using Server.AbstractFactory;
using Server.Command;
using Server.Observer;
using Server.Strategy.ShipPlacement;
using System.Collections.Generic;

namespace Server.Proxy
{
    public class GameProxy : IGame
    {
        private Game _realGame;

        private Game GetRealGame()
        {
            if (_realGame == null)
            {
                _realGame = new Game();
                _realGame.Initialize();
            }
            return _realGame;
        }

        public GameState State
        {
            get => GetRealGame().State;
            set => GetRealGame().State = value;
        }

        public IPlayer[] Players
        {
            get => GetRealGame().Players;
            set => GetRealGame().Players = value;
        }

        public int Turn
        {
            get => GetRealGame().Turn;
            set => GetRealGame().Turn = value;
        }

        public bool JoinPlayer(string name, int seat, IShipPlacementStrategy strategy)
        {
            return GetRealGame().JoinPlayer(name, seat, strategy);
        }

        public int setBoardChoice()
        {
            return GetRealGame().setBoardChoice();
        }

        public bool DisconnectPlayer(int seat)
        {
            return GetRealGame().DisconnectPlayer(seat);
        }

        public bool ReadyUp(int player)
        {
            return GetRealGame().ReadyUp(player);
        }

        public ShipHelper SetShip(int player, int size, int x, int y, bool vertical)
        {
            return GetRealGame().SetShip(player, size, x, y, vertical);
        }

        public Shot Fire(int attacker, int x, int y)
        {
            return GetRealGame().Fire(attacker, x, y);
        }

        public bool AreBothPlayersConnected()
        {
            return GetRealGame().AreBothPlayersConnected();
        }

        public bool AreBothPlayersReady()
        {
            return GetRealGame().AreBothPlayersReady();
        }

        public int CheckWin(int player)
        {
            return GetRealGame().CheckWin(player);
        }

        public ICommand CheckLastCommand(int player)
        {
            return GetRealGame().CheckLastCommand(player);
        }

        public void Undo(int player)
        {
            GetRealGame().Undo(player);
        }

        public void Attach(IObserver observer)
        {
            GetRealGame().Attach(observer);
        }

        public void Detach(IObserver observer)
        {
            GetRealGame().Detach(observer);
        }

        public void Notify()
        {
            GetRealGame().Notify();
        }

        public void SavePlayerState(int player)
        {
            GetRealGame().SavePlayerState(player);
        }

        public List<List<bool>> RestorePlayerState(int player)
        {
            return GetRealGame().RestorePlayerState(player);
        }

        public void Start()
        {
            State = GameState.Started;
            Turn = 0;
        }

        public void Initialize()
        {
            GetRealGame();
        }
    }
}
