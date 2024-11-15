using Server.Prototype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
	public class ShipCloneTests
	{
		[Test]
		public void Clone_ShouldCreateExactCopyOfShip_WithDifferentMemoryReference()
		{
			// Arrange: Create an instance of Destroyer with properties set
			ShipAbstract originalShip = new Destroyer();

			// Act: Clone the original ship
			ShipAbstract clonedShip = (ShipAbstract)originalShip.Clone();

			// Assert: Check that cloned ship is not null
			Assert.IsNotNull(clonedShip, "The cloned ship should not be null.");

			// Assert: Verify that the cloned ship has the same properties as the original
			Assert.That(clonedShip.Name, Is.EqualTo(originalShip.Name), "Cloned ship should have the same Name as the original.");
			Assert.That(clonedShip.Size, Is.EqualTo(originalShip.Size), "Cloned ship should have the same Size as the original.");
			Assert.That(clonedShip.Symbol, Is.EqualTo(originalShip.Symbol), "Cloned ship should have the same Symbol as the original.");

			// Assert: Verify that the cloned ship is a different instance in memory
			Assert.That(clonedShip, Is.Not.SameAs(originalShip), "The cloned ship should be a different object instance in memory.");
		}
	}
}
