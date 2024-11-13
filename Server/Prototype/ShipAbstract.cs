using System;

namespace Server.Prototype
{
	public abstract class ShipAbstract : ICloneable
	{
		public static readonly byte None = 0; // Represents an empty or cleared cell
		public string Name { get; set; }
		public int Size { get; protected set; }
		public byte Symbol { get; protected set; }
		public int IsHit = 0;

		public void MarkAsHit()
        {
            IsHit++;
        }

        // Checks if the ship has been hit
        public bool CheckIfSunk()
        {
            return Size==IsHit;
        }

		public bool IsValid()
		{
			return !string.IsNullOrEmpty(Name) && Size > 0 && Symbol > 0;
		}


		// Clone method for Prototype pattern
		public abstract object Clone();
	}
}
