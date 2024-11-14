using Server.Prototype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests
{
	public class ShipFactoryTests
	{
		private ShipFactory _shipFactory;

		[SetUp]
		public void Setup()
		{
			_shipFactory = new ShipFactory();
		}

		[Test]
		public void GetShip_ShouldReturnDestroyer_WhenShipTypeIsDestroyer()
		{
			// Act
			var result = _shipFactory.GetShip("Destroyer");

			// Assert
			Assert.IsNotNull(result, "Expected a non-null result for Destroyer.");
			Assert.IsInstanceOf<Destroyer>(result, "Expected the returned ship to be of type Destroyer.");
		}

		[Test]
		public void GetShip_ShouldReturnSubmarine_WhenShipTypeIsSubmarine()
		{
			// Act
			var result = _shipFactory.GetShip("Submarine");

			// Assert
			Assert.IsNotNull(result, "Expected a non-null result for Submarine.");
			Assert.IsInstanceOf<Submarine>(result, "Expected the returned ship to be of type Submarine.");
		}

		[Test]
		public void GetShip_ShouldReturnCruiser_WhenShipTypeIsCruiser()
		{
			// Act
			var result = _shipFactory.GetShip("Cruiser");

			// Assert
			Assert.IsNotNull(result, "Expected a non-null result for Cruiser.");
			Assert.IsInstanceOf<Cruiser>(result, "Expected the returned ship to be of type Cruiser.");
		}

		[Test]
		public void GetShip_ShouldReturnBattleship_WhenShipTypeIsBattleship()
		{
			// Act
			var result = _shipFactory.GetShip("Battleship");

			// Assert
			Assert.IsNotNull(result, "Expected a non-null result for Battleship.");
			Assert.IsInstanceOf<Battleship>(result, "Expected the returned ship to be of type Battleship.");
		}

		[Test]
		public void GetShip_ShouldReturnNull_WhenShipTypeIsInvalid()
		{
			// Act
			var result = _shipFactory.GetShip("Frigate");

			// Assert
			Assert.IsNull(result, "Expected null for an unsupported ship type.");
		}

	}
}
