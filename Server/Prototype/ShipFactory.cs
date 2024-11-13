using System.Collections.Generic;

namespace Server.Prototype
{
	public class ShipFactory
	{
		private readonly Dictionary<string, ShipAbstract> _shipPrototypes;

		public ShipFactory()
		{
			// Initialize the prototypes
			_shipPrototypes = new Dictionary<string, ShipAbstract>
			{
				{ "Destroyer", new Destroyer() },
				{ "Submarine", new Submarine() },
				{ "Cruiser", new Cruiser() },
				{ "Battleship", new Battleship() }
			};
		}

		// Get a clone of the requested ship type
		public ShipAbstract GetShip(string shipType)
		{
			if (_shipPrototypes.ContainsKey(shipType))
			{
				return (ShipAbstract)_shipPrototypes[shipType].Clone();
			}
			return null; // Or throw an exception for unsupported types
		}
	}
}
