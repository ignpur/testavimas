using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Strategy.ShipPlacement
{
    public class ManualShipPlacementStrategy : IShipPlacementStrategy
    {
        public ShipHelper PlaceShips(Player player, int size, int x, int y, bool vertical)
        {
            var ship = Ship.GetShipBySize(size);
            if (ship == Ship.None) return new ShipHelper(false,0,0,0,false);
            Console.WriteLine("Placed ship at {0}, {1}, vertical: {2}, size: {3}", x, y, vertical, size);
            return new ShipHelper(player.SetShip(ship, x, y, vertical), size, x, y, vertical);
        }
    }
}
