using System;
using Server;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.AbstractFactory;

namespace Server.Decorator
{
    public class ExtraShotDecorator : PlayerDecorator
    {
        private bool extraShotAvailable;

        public ExtraShotDecorator(IPlayer player) : base(player)
        {
            extraShotAvailable = false;
        }

        public override Shot HandleIncomingFire(int x, int y)
        {
            // Call the original HandleIncomingFire logic
            Shot shot = _decoratedPlayer.HandleIncomingFire(x, y);

            // Check if the shot missed and if the extra shot hasn't been used yet
            if (!shot.result && !extraShotAvailable)
            {
                Console.WriteLine($"{_decoratedPlayer.getName()} missed and gets an extra shot!");

                // Set the flag to true so the player only gets one extra shot
                extraShotAvailable = true;
            }

            return shot;
        }

        public void ResetExtraShot()
        {
            // Reset the extra shot usage flag at the start of each turn
            extraShotAvailable = false;
        }

        public bool IsExtraShotAvailable()
        {
            return extraShotAvailable;
        }
    }
}
