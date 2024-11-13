using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.AbstractFactory;
using Server.Prototype;
using Server.Strategy.ShipPlacement;

namespace Server.Command
{
    public class PlaceShipCommand : IResultCommand<ShipHelper>
    {
        private readonly IPlayer _player;
        private readonly int _size;
        private readonly int _x;
        private readonly int _y;
        private readonly bool _vertical;
        private ShipHelper _placedShip;

        public PlaceShipCommand(IPlayer player, int size, int x, int y, bool vertical)
        {
            _player = player;
            _size = size;
            _x = x;
            _y = y;
            _vertical = vertical;
        }

        public void Execute()
        {
            _placedShip = _player.PlaceShips(_size, _x, _y, _vertical);
        }

        public void Undo()
        {
            // Undo the ship placement by clearing the cells that _placedShip occupies
            if (_placedShip == null) return;

            if (_placedShip.vertical)
            {
                for (int i = _placedShip.posY; i < _placedShip.posY + _placedShip.size; i++)
                {
                    _player.GetBoard()[i, _placedShip.posX] = null;
                }
            }
            else
            {
                for (int i = _placedShip.posX; i < _placedShip.posX + _placedShip.size; i++)
                {
                    _player.GetBoard()[_placedShip.posY, i] = null;
                }
            }

            // Update the player’s ship count
            _player.DecrementShipCount(_size);
        }
        public ShipHelper GetResults()
        {
            return _placedShip;
        }
    }
}
