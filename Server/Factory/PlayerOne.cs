using Server.Strategy.ShipPlacement;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection.Metadata.Ecma335;
using Server.Prototype;

namespace Server.AbstractFactory
{
    public class PlayerOne : IPlayer
    {
        public string Name { get; }
        public bool Ready { get; private set; }
        public ShipAbstract[,] Board { get; private set; }
        public int[] ArrangedShipsCount { get; private set; }

        private IShipPlacementStrategy _placementStrategy;

        public string getName() {
            return this.Name;
        }
        public bool getReady() {
            return this.Ready;
        }
        public PlayerOne(string name, IShipPlacementStrategy placementStrategy)
        {
            Name = name;
            _placementStrategy = placementStrategy;
            Initialize();
        }

        public void SetBoardSize(int choice)
        {
			int size = 0;
            switch (choice)
			{
				case 1:
					size = 10;
                    break;
				case 2:
					size = 15;
					break;
				case 3:
					size = 20;
					break;
				default:
					size = 10;
					break;
			}
			Board = new ShipAbstract[size, size];

        }
        public void Initialize()
        {
            Ready = false;
            //Board = new byte[10, 10];
            ArrangedShipsCount = new int[4];
        }
        public ShipHelper PlaceShips(int size, int x, int y, bool vertical)
        {
            return _placementStrategy.PlaceShips(this, size, x, y, vertical);
        }

        /// <summary>
        /// Marks the player as ready to start the game
        /// </summary>
        /// <returns>
        /// false if player hasn't set all of their ships
        /// true otherwise
        /// </returns>
        public bool ReadyUp()
        {
            // TODO: refactor this method so it looks clearer
            Ready = (ArrangedShipsCount[0] == 4)
                && (ArrangedShipsCount[1] == 3)
                && (ArrangedShipsCount[2] == 2)
                && (ArrangedShipsCount[3] == 1);

            return Ready;
        }

        public bool SetShip(byte ship, int x, int y, bool vertical)
        {
            ShipAbstract shipAbstract = null;
            switch (ship)
            {
                case 1:
                    shipAbstract = new Destroyer();
                    break;
                case 2:
                    shipAbstract = new Submarine();
                    break;
                case 4:
                    shipAbstract = new Cruiser();
                    break;
                case 8:
                    shipAbstract = new Battleship();
                    break;
                default:
                    break;
            }

            if (!IsPositionInBounds(x, y)) return false;
            if (shipAbstract == null) return false;

            var size = shipAbstract.Size;
            if (!ValidatePlacement(x, y, size, vertical)) return false;
            if (ArrangedShipsCount[size - 1] == 5 - size) return false;

            if (vertical)
            {
                for (int i = y; i < y + size; i++)
                {
                    Board[i, x] = shipAbstract;
                }
            }
            else
            {
                for (int i = x; i < x + size; i++)
                {
                    Board[y, i] = shipAbstract;
                }
            }

            ArrangedShipsCount[size - 1]++;
            return true;
        }

        /// <summary>
        /// Handles incoming fire from the enemy
        /// </summary>
        /// <param name="x">X coordinate of the shot</param>
        /// <param name="y">Y coordinate of the shot</param>
        /// <returns>
        /// false if the ship was not hit
        /// true otherwise
        /// </returns>
        public Shot HandleIncomingFire(int x, int y)
        {
            if (!IsPositionInBounds(x, y)) return new Shot(x, y, false);

            var ship = Board[y, x];
            if (ship == null) return new Shot(x, y, false);
            if (ship.CheckIfSunk()) return new Shot(x, y, false);

            ship.MarkAsHit();
            return new Shot(x, y, true);
        }

        public bool HasEnabledShips()
        {
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    var ship = Board[i, j];
                    if (ship != null && !ship.CheckIfSunk()) return true;
                }
            }

            return false;
        }

        private bool IsPositionInBounds(int x, int y)
        {
            return ((x >= 0) && (x < Board.GetLength(0)) && (y >= 0) && (y <= Board.GetLength(1)));
        }

        private bool ValidatePlacement(int x, int y, int size, bool vertical)
        {
            if (x < 0 || x > Board.GetLength(0) || y < 0 || y > Board.GetLength(1)) return false;

            //Does the ship fit inside the bounds?
            if ((vertical && y + size - 1 > Board.GetLength(1) - 1) || (!vertical && x + size - 1 > Board.GetLength(0) - 1)) return false;

            var x1 = x - 1;
            var x2 = vertical ? x + 1 : x + size;
            var y1 = y - 1;
            var y2 = vertical ? y + size : y + 1;

            for (int i = y1 + 1; i <= y2 - 1; i++)
            {
                if (i < 0 || i > Board.GetLength(1) - 1) continue;
                for (int j = x1 + 1; j <= x2 - 1; j++)
                {
                    if (j < 0 || j > Board.GetLength(1) - 1) continue;
                    if (Board[i, j] != null) return false;
                }
            }

            return true;
        }
        public ShipAbstract[,] GetBoard() => Board;

        public void DecrementShipCount(int size) => ArrangedShipsCount[size-1]--;
    }
}
