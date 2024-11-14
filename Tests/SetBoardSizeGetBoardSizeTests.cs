using Server;
using Server.Strategy.ShipPlacement;
using Server.AbstractFactory;
using Microsoft.VisualStudio.CodeCoverage;

namespace Tests
{
	public class SetBoardSizeGetBoardSizeTests
	{
		private PlayerOne _player;
		[SetUp]
		public void Setup()
		{
			_player = new PlayerOne("TestPlayer", new ManualShipPlacementStrategy());
		}

		[Test]
		public void SetBoardSize_StandardSize_ShouldInitialize10x10Board()
		{
			// Act
			_player.SetBoardSize(1);

			// Assert
			var board = _player.GetBoard();
			Assert.That(board.GetLength(0), Is.EqualTo(10), "The board should have 10 rows for standard size.");
			Assert.That(board.GetLength(1), Is.EqualTo(10), "The board should have 10 columns for standard size.");
		}

		[Test]
		public void SetBoardSize_LargeSize_ShouldInitialize15x15Board()
		{
			// Act
			_player.SetBoardSize(2);

			// Assert
			var board = _player.GetBoard();
			Assert.That(board.GetLength(0), Is.EqualTo(15), "The board should have 15 rows for large size.");
			Assert.That(board.GetLength(1), Is.EqualTo(15), "The board should have 15 columns for large size.");
		}

		[Test]
		public void SetBoardSize_ExtraLargeSize_ShouldInitialize20x20Board()
		{
			// Act
			_player.SetBoardSize(3);

			// Assert
			var board = _player.GetBoard();
			Assert.That(board.GetLength(0), Is.EqualTo(20), "The board should have 20 rows for extra large size.");
			Assert.That(board.GetLength(1), Is.EqualTo(20), "The board should have 20 columns for extra large size.");
		}

		[Test]
		public void SetBoardSize_InvalidChoice_ShouldDefaultTo10x10Board()
		{
			// Act
			_player.SetBoardSize(0);

			// Assert
			var board = _player.GetBoard();
			Assert.That(board.GetLength(0), Is.EqualTo(10), "The board should default to 10 rows for invalid choice.");
			Assert.That(board.GetLength(1), Is.EqualTo(10), "The board should default to 10 columns for invalid choice.");
		}

	}
}