using Server.Strategy.ShipPlacement;
using Server;
using Server.Prototype;
using Server.Memento;
using System.Collections.Generic;

namespace Server.AbstractFactory
{
    public interface IPlayer
    {
        public string getName();
        public bool getReady();
        public void Initialize();
        public ShipHelper PlaceShips(int size, int x, int y, bool vertical);
        public bool ReadyUp();
        public bool SetShip(byte ship, int x, int y, bool vertical);
        public Shot HandleIncomingFire(int x, int y);
        public bool HasEnabledShips();
        public ShipAbstract[,] GetBoard();
        public void DecrementShipCount(int size);
        public void SetBoardSize(int choice);
        ShipPlacementMemento SaveState();
        void RestoreState(ShipPlacementMemento memento);
        public List<List<bool>> GetBoardUI();

    }
}
