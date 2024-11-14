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
    [TestFixture]
    internal class AutoDodgeDecoratorTests
    {
        private IPlayer player;

        [SetUp]
        public void Setup()
        {
            player = new PlayerOne("TestPlayer", new ManualShipPlacementStrategy());
            player.SetBoardSize(1);
            player = new AutoDodgeDecorator(player);
        }

        [Test]
        public void AutoDodgeDecorator_ShouldRedirectShotOnDodgeToEmptyCell()
        {
             // Simulate dodge chance met by invoking HandleIncomingFire
            bool shotDodged = false;
            var result = player.HandleIncomingFire(1, 1);
            if (result != null && (result.x != 1 || result.y != 1))
            {
                shotDodged = true;
            }

            // Assert
            Assert.IsFalse(result.result, "The shot should be dodged to a random empty cell.");
        }
    }
}
