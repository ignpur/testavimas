/*using Server;
using Server.Strategy.ShipPlacement;
using Server.AbstractFactory;
namespace Tests
{
    public class Tests
    {
        private PlayerOne _player;
        [SetUp]
        public void Setup()
        {
            _player = new PlayerOne("TestPlayer", new ManualShipPlacementStrategy());
        }

        [Test]
        public void TestShipPlacement_ValidCoordinates_ShouldPlaceShipSuccessfully()
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
            for (int i = x; i < x + size; i++)
            {
                Assert.AreEqual(Ship.GetShipBySize(size), _player.Board[y, i], "The board should have the ship placed horizontally.");
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

            // Act
            ShipHelper result = _player.PlaceShips(size, x, y, vertical);

            // Assert
            Assert.IsFalse(result.result, "The ship placement should fail due to out-of-bounds placement.");
        }

        [Test]
        public void TestShipPlacement_Overlap_ShouldFailToPlaceShip()
        {
            // Arrange: Place the first ship
            _player.PlaceShips(Ship.GetShipBySize(3), 0, 0, false);

            // Act: Attempt to place another ship overlapping the first one
            ShipHelper result = _player.PlaceShips(Ship.GetShipBySize(2), 0, 0, true);

            // Assert
            Assert.IsFalse(result.result, "The ship placement should fail due to overlap.");
        }
	}
}*/