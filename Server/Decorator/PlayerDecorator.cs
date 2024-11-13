using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.AbstractFactory;
using Server.Prototype;
using Server.Strategy.ShipPlacement;

namespace Server.Decorator
{
    public abstract class PlayerDecorator : IPlayer
    {
        protected IPlayer _decoratedPlayer;

        protected PlayerDecorator(IPlayer player)
        {
            _decoratedPlayer = player;
        }

        public virtual string getName() => _decoratedPlayer.getName();

        public virtual void Initialize() => _decoratedPlayer.Initialize();

        public virtual ShipHelper PlaceShips(int size, int x, int y, bool vertical) => _decoratedPlayer.PlaceShips(size, x, y, vertical);

        public virtual Shot HandleIncomingFire(int x, int y) => _decoratedPlayer.HandleIncomingFire(x, y);

        public virtual bool HasEnabledShips() => _decoratedPlayer.HasEnabledShips();

        public virtual bool ReadyUp() => _decoratedPlayer.ReadyUp();

        public virtual bool getReady() => _decoratedPlayer.getReady();
        public bool SetShip(byte ship, int x, int y, bool vertical) => _decoratedPlayer.SetShip(ship, x, y, vertical);
        public ShipAbstract[,] GetBoard() => _decoratedPlayer.GetBoard();
        public void DecrementShipCount(int size) => _decoratedPlayer.DecrementShipCount(size);

		public void SetBoardSize(int choice) => _decoratedPlayer.SetBoardSize(choice);
	}
}
