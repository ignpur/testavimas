using Server.AbstractFactory;
using Server.Strategy.ShipPlacement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    internal class RandomShipPlacementTests
    {
        private PlayerOne _player;
        [SetUp]
        public void Setup()
        {
            _player = new PlayerOne("TestPlayer", new RandomShipPlacementStrategy());
            _player.SetBoardSize(1); //Set board size of 10x10
        }

        [Test]
        public void TestValidShipPlacement()
        {
            // Arrange: Ship of size 3 at (0, 0) horizontally
            int size = 3;
            int x = 0;
            int y = 0;
            bool vertical = false;

            // Act
            ShipHelper result = _player.PlaceShips(size, x, y, vertical);

            // Assert
            Assert.IsTrue(result.result, "The ship should be placed successfully.");
            if (result.vertical)
            {
                for (int i = result.posY; i < result.posY + result.size; i++)
                {
                    Assert.IsTrue(_player.Board[i, result.posX].IsValid(), "The board should have the ship placed horizontally.");
                }
            }
            else
            {
                for (int i = result.posX; i < result.posX + result.size; i++)
                {
                    Assert.IsTrue(_player.Board[result.posY, i].IsValid(), "The board should have the ship placed horizontally.");
                }
            }
        }

        [Test]
        public void TestShipPlacement_InvalidCoordinates_ShouldPlaceInEmptySpot()
        {
            // Arrange: Ship of size 4 outside the bounds at (8, 0) horizontally
            int size = 4;
            int x = 8;
            int y = 0;
            bool vertical = false;

            // Act
            ShipHelper result = _player.PlaceShips(size, x, y, vertical);

            // Assert
            Assert.IsTrue(result.result, "The ship placement should not fail due to out-of-bounds placement, because random placement strategy selects available empty slot.");
        }

        [Test]
        public void TestShipPlacement_Overlap_ShouldPlaceShipInEmptySpot()
        {
            // Arrange: Place the first ship
            _player.PlaceShips(3, 0, 0, false);

            // Act: Attempt to place another ship overlapping the first one
            ShipHelper result = _player.PlaceShips(2, 0, 0, true);

            // Assert
            Assert.IsTrue(result.result, "The ship placement should not fail due to overlap, because random placement strategy selects available empty slot.");
        }
    }
}
