using Server.AbstractFactory;
using Server.Prototype;
using Server.Strategy.ShipPlacement;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    internal class SetShipTests
    {
        private PlayerOne _player;
        ShipAbstract shipAbstract = null;
        [SetUp]
        public void Setup()
        {
            _player = new PlayerOne("TestPlayer", new ManualShipPlacementStrategy());
            _player.SetBoardSize(1); //Set board size of 10x10
        }

        [Test]
        public void TestShipPlacement_ValidCoordinates_ShouldPlaceShipSuccessfully()
        {
            // Arrange: Ship of size 3 at (0, 0) horizontally
            int size = 3;
            int x = 0;
            int y = 0;
            bool vertical = false;

            shipAbstract = new Cruiser();
            var ship = shipAbstract.Symbol;

            // Act
            bool result = _player.SetShip(ship, x, y, vertical);

            // Assert
            Assert.IsTrue(result, "The ship should be placed successfully.");
            for (int i = x; i < x + size; i++)
            {
                Assert.IsTrue(_player.Board[y, i].IsValid(), "The board should have the ship placed horizontally.");
            }
        }

        [Test]
        public void TestShipPlacement_InvalidCoordinates_ShouldFailToPlaceShip()
        {
            // Arrange: Ship of size 4 outside the bounds at (8, 0) horizontally
            int size = 4;
            int x = 8;
            int y = 0;
            bool vertical = false;

          
            shipAbstract = new Battleship();
            var ship = shipAbstract.Symbol;

            // Act
            bool result = _player.SetShip(ship, x, y, vertical);

            // Assert
            Assert.IsFalse(result, "The ship placement should fail due to out-of-bounds placement.");
        }

        [Test]
        public void TestShipPlacement_Overlap_ShouldFailToPlaceShip()
        {
            shipAbstract = new Cruiser();
            var ship = shipAbstract.Symbol;

            // Arrange: Place the first ship
            _player.SetShip(ship, 0, 0, false);

            shipAbstract = new Submarine();
            var ship2 = shipAbstract.Symbol;

            // Act: Attempt to place another ship overlapping the first one
            ShipHelper result = _player.PlaceShips(ship2, 0, 0, true);

            // Assert
            Assert.IsFalse(result.result, "The ship placement should fail due to overlap.");
        }
    }
}
