using Server.Strategy.ShipPlacement;
using Server;
using Server.Prototype;

namespace Server.AbstractFactory
{
    public interface IPlayer
    {
        string getName();
        bool getReady();
        void Initialize();
        ShipHelper PlaceShips(int size, int x, int y, bool vertical);
        bool ReadyUp();
        bool SetShip(byte ship, int x, int y, bool vertical);
        Shot HandleIncomingFire(int x, int y);
        bool HasEnabledShips();
        ShipAbstract[,] GetBoard();
        void DecrementShipCount(int size);
        void SetBoardSize(int choice);
    }

}
