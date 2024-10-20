using System;

namespace Server.Strategy.ShipPlacement
{
    public class MixedShipPlacementStrategy : IShipPlacementStrategy
    {
        private static Random _random = new Random();

        public ShipHelper PlaceShips(Player player, int size, int x, int y, bool vertical)
        {
            bool placed = false;
            int randomX = 0, randomY = 0;
            bool randomVertical = false;
            int timeOutCounter = 0;
            while (!placed && timeOutCounter < 12000)
            {
                timeOutCounter++;
                randomX = _random.Next(0, 10);
                randomY = _random.Next(0, 10);
                randomVertical = _random.Next(2) == 0;
                var ship = Ship.GetShipBySize(size);
                placed = player.SetShip(ship, randomX, randomY, randomVertical);
            }
            Console.WriteLine("Placed ship at {0}, {1}, vertical: {2}, size: {3}, timeoutcounter: {4}", randomX, randomY, randomVertical, size, timeOutCounter);

            return new ShipHelper(placed, size, randomX, randomY, randomVertical);
        }
    }
}
