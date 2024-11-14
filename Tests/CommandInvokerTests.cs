using Server.AbstractFactory;
using Server.Command;
using Server.Prototype;
using Server.Strategy.ShipPlacement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    internal class CommandInvokerTests
    {
        private CommandInvoker invoker;
        private IPlayer player;
        private PlaceShipCommand placeShipCommand;

        [SetUp]
        public void Setup()
        {
            invoker = new CommandInvoker();
            player = new PlayerOne("TestPlayer", new ManualShipPlacementStrategy());
            player.SetBoardSize(1);


            placeShipCommand = new PlaceShipCommand(player, 3, 2, 2, false);
        }

        [Test]
        public void ExecuteCommand_ShouldPlaceShipOnBoard()
        {
            // Act
            invoker.ExecuteCommand(placeShipCommand);

            // Assert that command was executed and result was returned
            var lastCommand = invoker.CheckLast();
            Assert.AreEqual(placeShipCommand, lastCommand);

            var result = ((PlaceShipCommand)lastCommand).GetResults();
            Assert.IsTrue(result.result, "Ship should be placed successfully.");
            Assert.AreEqual(3, result.size);
            Assert.AreEqual(2, result.posX);
            Assert.AreEqual(2, result.posY);
            Assert.IsFalse(result.vertical);
        }

        [Test]
        public void Undo_ShouldRemoveShipFromBoard()
        {
            // Act
            invoker.ExecuteCommand(placeShipCommand);

            Assert.IsTrue(player.HasEnabledShips(), "Player should have one ship");
            invoker.Undo();
            Assert.IsFalse(player.HasEnabledShips(), "Player should have no ship");

            // Assert
        }

        [Test]
        public void CheckLast_ShouldReturnMostRecentCommand()
        {
            // Arrange
            invoker.ExecuteCommand(placeShipCommand);

            // Act
            var lastCommand = invoker.CheckLast();

            // Assert
            Assert.AreEqual(placeShipCommand, lastCommand, "CheckLast should return the last command executed.");
        }

        [Test]
        public void Undo_ShouldNotThrow_WhenNoCommandsExecuted()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => invoker.Undo(), "Undo should not throw an error when there are no commands to undo.");
        }
    }
}
