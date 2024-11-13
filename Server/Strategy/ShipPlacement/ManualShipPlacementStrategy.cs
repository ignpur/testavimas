using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.AbstractFactory;
using Server.Prototype;

namespace Server.Strategy.ShipPlacement
{
    public class ManualShipPlacementStrategy : IShipPlacementStrategy
    {
        public ShipHelper PlaceShips(IPlayer player, int size, int x, int y, bool vertical)
        {
			ShipAbstract shipAbstract = null;
			switch (size)
			{
				case 1:
					shipAbstract = new Destroyer();
					break;
				case 2:
					shipAbstract = new Submarine();
					break;
				case 3:
					shipAbstract = new Cruiser();
					break;
				case 4:
					shipAbstract = new Battleship();
					break;
				default:
					break;
			}

			var ship = shipAbstract.Symbol;
            if (ship == ShipAbstract.None) return new ShipHelper(false,0,0,0,false);
            Console.WriteLine("Placed ship at {0}, {1}, vertical: {2}, size: {3}", x, y, vertical, size);
            return new ShipHelper(player.SetShip(ship, x, y, vertical), size, x, y, vertical);
        }
    }
}
