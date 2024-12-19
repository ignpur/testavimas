using Server.Proxy;
using System;

namespace Server.TemplateMethod
{
    public sealed class PlayerOneTurn : PlayerTurnTemplate
    {
        private IGame _game;
        private int _x, _y;

        public PlayerOneTurn(IGame game, int x, int y)
        {
            _game = game;
            _x = x;
            _y = y;
        }

        protected override void TakeShot()
        {
            Console.WriteLine("Player 1 fires at ({0}, {1})", _x, _y);
            _game.Fire(0, _x, _y); // Player 1 (index 0) fires
        }

        protected override void UpdateGameState()
        {
            Console.WriteLine("Updating game state for Player 1...");
            // No additional logic needed as Game.cs already processes the shot
        }
    }
}
