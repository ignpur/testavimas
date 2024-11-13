using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.AbstractFactory;
using Server;
using Server.Prototype;

namespace Server.Decorator
{
    public class AutoDodgeDecorator : PlayerDecorator
    {
        private readonly Random _random = new Random();
        private readonly int _dodgeChance = 3;  //Dodge chance is 1 divided by set value || 3 ~ 33%

        public AutoDodgeDecorator(IPlayer player) : base(player) { }

        public override Shot HandleIncomingFire(int x, int y)
        {
            if (DodgeShot())
            {
                Console.WriteLine($"{_decoratedPlayer.getName()} dodged the shot!");
                Shot redirectedShot = GetRandomEmptyCell();
                return base.HandleIncomingFire(redirectedShot.x, redirectedShot.y);
            }
            else
            {
                return base.HandleIncomingFire(x, y);
            }
            
        }
        private bool DodgeShot()
        {
            return _random.Next(_dodgeChance) == 0;
        }
        private Shot GetRandomEmptyCell()
        {
            var emptyCells = new List<Shot>();
            var board = GetBoard();

            for (int i = 0; i < board.GetLength(0); i++)
                for (int j = 0; j < board.GetLength(1); j++)
                    if (board[i, j] == null)
                        emptyCells.Add(new Shot(j, i, false));

            return emptyCells.Any() ? emptyCells[_random.Next(emptyCells.Count)] : null;
        }
    }
}
