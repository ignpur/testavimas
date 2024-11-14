using Server.Prototype;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
	public class ShipHitTest
	{
		private ShipAbstract _ship;

		[SetUp]
		public void Setup()
		{
			// Initialize a ship with size 3 for testing purposes.
			_ship = new TestShip("TestShip", 3); // Assume TestShip is a concrete class for testing, inheriting from ShipAbstract
		}

		[Test]
		public void MarkAsHit_ShouldIncrementIsHit()
		{
			// Act
			_ship.MarkAsHit();

			// Assert
			Assert.That(_ship.IsHit, Is.EqualTo(1), "Expected IsHit to be incremented to 1 after one hit.");
		}

		[Test]
		public void MarkAsHit_WhenCalledMultipleTimes_ShouldTrackHitsCorrectly()
		{
			// Act
			_ship.MarkAsHit();
			_ship.MarkAsHit();

			// Assert
			Assert.That(_ship.IsHit, Is.EqualTo(2), "Expected IsHit to be incremented to 2 after two hits.");
			Assert.IsFalse(_ship.CheckIfSunk(), "Expected CheckIfSunk to return false when the ship has not been fully hit.");
		}

		[Test]
		public void MarkAsHit_WhenHitsEqualSize_ShouldReturnSunk()
		{
			// Act
			_ship.MarkAsHit();
			_ship.MarkAsHit();
			_ship.MarkAsHit();

			// Assert
			Assert.IsTrue(_ship.CheckIfSunk(), "Expected CheckIfSunk to return true when the number of hits equals the ship's size.");
		}
	}

	// TestShip class implementation for testing purposes
	public class TestShip : ShipAbstract
	{
		public TestShip(string name, int size)
		{
			Name = name;
			Size = size;
			Symbol = 1; // Arbitrary non-zero symbol
		}

		public override object Clone()
		{
			return new TestShip(Name, Size);
		}
	}
}
