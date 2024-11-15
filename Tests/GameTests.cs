using Moq;
using Server;
using Server.AbstractFactory;
using Server.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
	public class GameTests
	{
		private Game _game;
		private Mock<IPlayer> _playerMock1;
		private Mock<IPlayer> _playerMock2;

		[SetUp]
		public void Setup()
		{
			_game = new Game();

			_playerMock1 = new Mock<IPlayer>();
			_playerMock2 = new Mock<IPlayer>();

			_game.Players[0] = _playerMock1.Object;
			_game.Players[1] = _playerMock2.Object;

		}

		[Test]
		public void TestSetBoardChoice_ValidRange()
		{
			// Act
			int boardChoice = _game.setBoardChoice();

			// Assert
			Assert.That(boardChoice, Is.InRange(1, 3), "The board choice should be between 1 and 3.");
		}

		[Test]
		public void TestSetBoardChoice_ReturnsSameValueOnSubsequentCalls()
		{
			// Act
			int firstChoice = _game.setBoardChoice();
			int secondChoice = _game.setBoardChoice();

			// Assert
			Assert.That(secondChoice, Is.EqualTo(firstChoice), "Subsequent calls should return the same board choice if not reset.");
		}

		[Test]
		public void DisconnectPlayer_InvalidSeat_ReturnsFalse()
		{
			// Act
			bool resultNegativeSeat = _game.DisconnectPlayer(-1);
			bool resultOutOfRangeSeat = _game.DisconnectPlayer(2);

			// Assert
			Assert.IsFalse(resultNegativeSeat, "Expected false for an invalid seat (-1).");
			Assert.IsFalse(resultOutOfRangeSeat, "Expected false for an out-of-range seat (2).");
		}
		[Test]
		public void DisconnectPlayer_ValidSeat0_ReturnsTrueAndResetsState()
		{
			// Act
			bool result = _game.DisconnectPlayer(0);

			// Assert
			Assert.IsTrue(result, "Expected true for valid seat disconnection.");
			Assert.IsNull(_game.Players[0], "Player in seat 0 should be set to null.");
			Assert.That(_game.State, Is.EqualTo(GameState.NotStarted), "Game state should be set to NotStarted.");
			_playerMock2.Verify(p => p.Initialize(), Times.Once, "Expected the other player to be reinitialized.");
		}

		[Test]
		public void DisconnectPlayer_ValidSeat1_ReturnsTrueAndResetsState()
		{
			// Act
			bool result = _game.DisconnectPlayer(1);

			// Assert
			Assert.IsTrue(result, "Expected true for valid seat disconnection.");
			Assert.IsNull(_game.Players[1], "Player in seat 1 should be set to null.");
			Assert.That(_game.State, Is.EqualTo(GameState.NotStarted), "Game state should be set to NotStarted.");
			_playerMock1.Verify(p => p.Initialize(), Times.Once, "Expected the other player to be reinitialized.");
		}
		[Test]
		public void ReadyUp_InvalidSeat_ReturnsFalse()
		{
			// Act
			bool resultNegativeSeat = _game.ReadyUp(-1);
			bool resultOutOfRangeSeat = _game.ReadyUp(2);

			// Assert
			Assert.IsFalse(resultNegativeSeat, "Expected false for an invalid seat (-1).");
			Assert.IsFalse(resultOutOfRangeSeat, "Expected false for an out-of-range seat (2).");
		}

		[Test]
		public void ReadyUp_ValidSeatWithoutPlayer_ReturnsFalse()
		{
			// Act
			bool result = _game.ReadyUp(0);

			// Assert
			Assert.IsFalse(result, "Expected false because there is no player in seat 0.");
		}

		[Test]
		public void ReadyUp_ValidSeatWithPlayer_ReturnsTrue()
		{
			// Arrange
			_playerMock1.Setup(p => p.ReadyUp()).Returns(true);
			_game.Players[0] = _playerMock1.Object;

			// Act
			bool result = _game.ReadyUp(0);

			// Assert
			Assert.IsTrue(result, "Expected true because the player in seat 0 is marked as ready.");
			_playerMock1.Verify(p => p.ReadyUp(), Times.Once, "Expected ReadyUp to be called on the player in seat 0.");
		}

		[Test]
		public void ReadyUp_ValidSeatWithUnreadyPlayer_ReturnsFalse()
		{
			// Arrange
			_playerMock1.Setup(p => p.ReadyUp()).Returns(false);
			_game.Players[0] = _playerMock1.Object;

			// Act
			bool result = _game.ReadyUp(0);

			// Assert
			Assert.IsFalse(result, "Expected false because the player in seat 0 is not ready.");
			_playerMock1.Verify(p => p.ReadyUp(), Times.Once, "Expected ReadyUp to be called on the player in seat 0.");
		}
		[Test]
		public void CheckWin_InvalidPlayer_ReturnsNegativeOne()
		{
			// Act
			int resultNegative = _game.CheckWin(-1);
			int resultOutOfRange = _game.CheckWin(2);

			// Assert
			Assert.That(resultNegative, Is.EqualTo(-1), "Expected -1 for an invalid player (-1).");
			Assert.That(resultOutOfRange, Is.EqualTo(-1), "Expected -1 for an out-of-range player (2).");
		}

		[Test]
		public void CheckWin_OpposingPlayerHasEnabledShips_ReturnsZero()
		{
			// Arrange
			_playerMock2.Setup(p => p.HasEnabledShips()).Returns(true);

			// Act
			int result = _game.CheckWin(0);

			// Assert
			Assert.That(result, Is.EqualTo(0), "Expected 0 as the opposing player has enabled ships.");
			_playerMock2.Verify(p => p.HasEnabledShips(), Times.Once, "Expected HasEnabledShips to be called on the opposing player.");
		}

		[Test]
		public void CheckWin_OpposingPlayerHasNoEnabledShips_ReturnsOne()
		{
			// Arrange
			_playerMock2.Setup(p => p.HasEnabledShips()).Returns(false);

			// Act
			int result = _game.CheckWin(0);

			// Assert
			Assert.That(result, Is.EqualTo(1), "Expected 1 as the opposing player has no enabled ships.");
			_playerMock2.Verify(p => p.HasEnabledShips(), Times.Once, "Expected HasEnabledShips to be called on the opposing player.");
		}
	}
}
