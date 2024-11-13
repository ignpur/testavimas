using System.Drawing;
using System.Xml.Linq;

namespace Server.Prototype
{
	public class Submarine : ShipAbstract
	{
		public Submarine()
		{
			Name = "Submarine";
			Size = 2;
			Symbol = 2;
		}

		public override object Clone()
		{
			return MemberwiseClone();
		}
	}
}
