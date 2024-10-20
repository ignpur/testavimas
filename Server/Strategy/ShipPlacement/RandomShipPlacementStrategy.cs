using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Strategy.ShipPlacement
{
    public class RandomShipPlacementStrategy : IShipPlacementStrategy
    {
        private static Random _random = new Random();

        public ShipHelper PlaceShips(Player player, int size, int x, int y, bool vertical)
        {
            bool placed = false;
            int randomX = 0, randomY = 0;
            int randomSize = 0;
            bool randomVertical = false;
            int timeOutCounter = 0;
            while (!placed && timeOutCounter < 12000)
            {
                timeOutCounter++;
                randomX = _random.Next(0, 10);
                randomY = _random.Next(0, 10);
                randomVertical = _random.Next(2) == 0;
                randomSize = _random.Next(1, 5);
                var ship = Ship.GetShipBySize(randomSize);
                placed = player.SetShip(ship, randomX, randomY, randomVertical);
            }
            Console.WriteLine("Placed ship at {0}, {1}, vertical: {2}, size: {3}, counter: {4}", randomX, randomY, randomVertical, randomSize, timeOutCounter);

            return new ShipHelper(placed, randomSize, randomX, randomY, randomVertical);
        }
    }
}
