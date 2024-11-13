using System.Drawing;
using System.Xml.Linq;

namespace Server.Prototype
{
	public class Destroyer : ShipAbstract
	{
		public Destroyer()
		{
			Name = "Destroyer";
			Size = 1;
			Symbol = 1;
		}

		public override object Clone()
		{
			return MemberwiseClone();
		}
	}
}
