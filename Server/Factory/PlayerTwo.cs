﻿using Server.Iterator;
using Server.Memento;
using Server.Prototype;
using Server.Strategy.ShipPlacement;
using System;
using System.Collections.Generic;

namespace Server.AbstractFactory
{
    public class PlayerTwo : IPlayer
    {
        public string Name { get; }
        public bool Ready { get; private set; }
        public ShipAbstract[,] Board { get; private set; }
        public int[] ArrangedShipsCount { get; private set; }

        private IShipPlacementStrategy _placementStrategy;

        public string getName()
        {
            return this.Name;
        }
        public bool getReady()
        {
            return this.Ready;
        }
        public PlayerTwo(string name, IShipPlacementStrategy placementStrategy)
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
            Ready = (ArrangedShipsCount[0] == 5)
                && (ArrangedShipsCount[1] == 4)
                && (ArrangedShipsCount[2] == 3)
                && (ArrangedShipsCount[3] == 2);

            return Ready;
        }

        public bool SetShip(byte ship, int startX, int startY, bool vertical)
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

			if (shipAbstract == null) return false;

			var size = shipAbstract.Size;

			var iterator = new BoardIterator(startX, startY, size, vertical);

			if (ArrangedShipsCount[size - 1] == 5 - size + 1) return false;

			if (!ValidatePlacement(startX, startY, size, vertical))
			{
				Console.WriteLine("Invalid placement: ship doesn't fit or overlaps.");
				return false;
			}


			// Reset the iterator and place the ship
			iterator = new BoardIterator(startX, startY, shipAbstract.Size, vertical);
			while (iterator.HasNext())
			{
				var (x, y) = iterator.Next();
				Board[y, x] = shipAbstract;
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
        public ShipPlacementMemento SaveState()
        {
            return new ShipPlacementMemento(Board, ArrangedShipsCount);
        }

        public void RestoreState(ShipPlacementMemento memento)
        {
            // Restore the board state
            Board = memento.BoardState.Clone() as ShipAbstract[,];

            // Restore the arranged ships count
            ArrangedShipsCount = (int[])memento.ArrangedShipsCount.Clone();
        }


        public List<List<bool>> GetBoardUI()
        {
            var boardState = new List<List<bool>>();
            for (int y = 0; y < Board.GetLength(0); y++)
            {
                var row = new List<bool>();
                for (int x = 0; x < Board.GetLength(1); x++)
                {
                    row.Add(Board[y, x] != null); // true for ship, false for empty
                }
                boardState.Add(row);
            }
            return boardState;
        }
        public ShipAbstract[,] GetBoard() => Board;

        public void DecrementShipCount(int size) => ArrangedShipsCount[size - 1]--;
    }
}
