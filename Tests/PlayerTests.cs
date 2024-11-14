using Server.AbstractFactory;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Server.Strategy.ShipPlacement;

namespace Tests
{

    public class PlayerTests
    {
        private IPlayer _playerOne;
        private IPlayer _playerTwo;

        [SetUp]
        public void Setup()
        {
            _playerOne = new PlayerOne("Player1", null);
            _playerTwo = new PlayerTwo("Player2", null);
        }

        [Test]
        public void GetName_ReturnsCorrectName()
        {
            Assert.That(_playerOne.getName(), Is.EqualTo("Player1"), "Expected name for PlayerOne is 'Player1'.");
            Assert.That(_playerTwo.getName(), Is.EqualTo("Player2"), "Expected name for PlayerTwo is 'Player2'.");
        }

        [Test]
        public void GetReady_InitiallyReturnsFalse()
        {
            Assert.IsFalse(_playerOne.getReady(), "PlayerOne should not be ready by default.");
            Assert.IsFalse(_playerTwo.getReady(), "PlayerTwo should not be ready by default.");
        }

        [Test]
        public void Initialize_ResetsPlayerState()
        {
            _playerOne.ReadyUp();
            _playerTwo.ReadyUp();

            _playerOne.Initialize();
            _playerTwo.Initialize();

            Assert.IsFalse(_playerOne.getReady(), "After Initialize, PlayerOne should not be ready.");
            Assert.IsFalse(_playerTwo.getReady(), "After Initialize, PlayerTwo should not be ready.");
        }

        [Test]
        public void ReadyUp_SetsPlayerToReadyState()
        {
            // Initialize board to avoid null references if accessed in ReadyUp
            _playerOne.SetBoardSize(1);
            _playerTwo.SetBoardSize(1);

            // Manually set `ArrangedShipsCount` based on each player’s requirements
            if (_playerOne is PlayerOne playerOneInstance)
            {
                playerOneInstance.ArrangedShipsCount[0] = 4;
                playerOneInstance.ArrangedShipsCount[1] = 3;
                playerOneInstance.ArrangedShipsCount[2] = 2;
                playerOneInstance.ArrangedShipsCount[3] = 1;
            }

            if (_playerTwo is PlayerTwo playerTwoInstance)
            {
                playerTwoInstance.ArrangedShipsCount[0] = 5;
                playerTwoInstance.ArrangedShipsCount[1] = 4;
                playerTwoInstance.ArrangedShipsCount[2] = 3;
                playerTwoInstance.ArrangedShipsCount[3] = 2;
            }

            Assert.IsTrue(_playerOne.ReadyUp(), "Expected PlayerOne to be ready after arranging ships.");
            Assert.IsTrue(_playerTwo.ReadyUp(), "Expected PlayerTwo to be ready after arranging ships.");

            Assert.IsTrue(_playerOne.getReady(), "Expected PlayerOne `getReady` to return true after `ReadyUp`.");
            Assert.IsTrue(_playerTwo.getReady(), "Expected PlayerTwo `getReady` to return true after `ReadyUp`.");
        }
    }
}
