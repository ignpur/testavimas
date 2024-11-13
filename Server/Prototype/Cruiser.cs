namespace Server.Prototype
{
	public class Cruiser : ShipAbstract
	{
		public Cruiser()
		{
			Name = "Cruiser";
			Size = 3;
			Symbol = 4;
		}

		public override object Clone()
		{
			return MemberwiseClone();
		}
	}
}
