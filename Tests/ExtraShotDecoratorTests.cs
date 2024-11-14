using Server;
using Server.AbstractFactory;
using Server.Decorator;
using Server.Strategy.ShipPlacement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    internal class ExtraShotDecoratorTests
    {
        private IPlayer player;
        private ExtraShotDecorator decorator;

        [SetUp]
        public void Setup()
        {
            player = new PlayerOne("TestPlayer", new ManualShipPlacementStrategy());
            player.SetBoardSize(1);
            decorator = new ExtraShotDecorator(player);
        }

        [Test]
        public void ExtraShotDecorator_ShouldGrantExtraShot_WhenShotMisses()
        {
            // Simulate a missed shot
            var shot = new Shot(0, 0, false); // miss
            decorator.HandleIncomingFire(shot.x, shot.y);

            // Assert
            Assert.IsTrue(decorator.IsExtraShotAvailable(), "The player should be granted an extra shot after a miss.");
        }
        [Test]
        public void ExtraShotDecorator_ShouldResetExtraShotAvailability_BetweenTurns()
        {
            // Arrange
            decorator = new ExtraShotDecorator(player);

            // Simulate missed shot and extra shot granted
            var shot = new Shot(0, 0, false);
            decorator.HandleIncomingFire(shot.x, shot.y);

            // Reset the extra shot at start of new turn
            decorator.ResetExtraShot();

            // Assert
            Assert.False(decorator.IsExtraShotAvailable(), "Extra shot availability should reset at the start of each turn.");
        }
       
    }
}
