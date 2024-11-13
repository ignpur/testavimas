using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.AbstractFactory;
using Server.Strategy.ShipPlacement;


namespace Server
{
    public interface IShipPlacementStrategy
    {
        ShipHelper PlaceShips(IPlayer player, int size, int x, int y, bool vertical);
    }
}
