using Server.Prototype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests
{
	public class ShipValidationTests
	{
		private ShipAbstract _ship;

		[Test]
		public void IsValid_ShouldReturnTrue_ForValidShip()
		{
			// Arrange
			_ship = new TestValidShips("Battleship", 4, 1);

			// Act
			bool result = _ship.IsValid();

			// Assert
			Assert.IsTrue(result, "Expected IsValid to return true for a valid ship.");
		}

		[Test]
		public void IsValid_ShouldReturnFalse_ForEmptyName()
		{
			// Arrange
			_ship = new TestValidShips("", 4, 1);

			// Act
			bool result = _ship.IsValid();

			// Assert
			Assert.IsFalse(result, "Expected IsValid to return false for a ship with an empty name.");
		}

		[Test]
		public void IsValid_ShouldReturnFalse_ForZeroSize()
		{
			// Arrange
			_ship = new TestValidShips("Destroyer", 0, 1);

			// Act
			bool result = _ship.IsValid();

			// Assert
			Assert.IsFalse(result, "Expected IsValid to return false for a ship with a size of 0.");
		}

		[Test]
		public void IsValid_ShouldReturnFalse_ForZeroSymbol()
		{
			// Arrange
			_ship = new TestValidShips("Cruiser", 3, 0);

			// Act
			bool result = _ship.IsValid();

			// Assert
			Assert.IsFalse(result, "Expected IsValid to return false for a ship with a symbol of 0.");
		}
	}

	// TestShip class implementation for testing purposes
	public class TestValidShips : ShipAbstract
	{
		public TestValidShips(string name, int size, byte symbol)
		{
			Name = name;
			Size = size;
			Symbol = symbol;
		}

		public override object Clone()
		{
			return new TestValidShips(Name, Size, Symbol);
		}
	}
}
