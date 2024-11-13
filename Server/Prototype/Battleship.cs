namespace Server.Prototype
{
	public class Battleship : ShipAbstract
	{
		public Battleship()
		{
			Name = "Battleship";
			Size = 4;
			Symbol = 8;
		}

		public override object Clone()
		{
			return MemberwiseClone();
		}
	}
}
