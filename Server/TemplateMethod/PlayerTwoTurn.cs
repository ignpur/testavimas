using Server.Proxy;
using System;

namespace Server.TemplateMethod
{
    public sealed class PlayerTwoTurn : PlayerTurnTemplate
    {
        private IGame _game;
        private int _x, _y;

        public PlayerTwoTurn(IGame game, int x, int y)
        {
            _game = game;
            _x = x;
            _y = y;
        }

        protected override void TakeShot()
        {
            Console.WriteLine("Player 2 fires at ({0}, {1})", _x, _y);
            _game.Fire(1, _x, _y); // Player 2 (index 1) fires
        }

        protected override void UpdateGameState()
        {
            Console.WriteLine("Updating game state for Player 2...");
        }
    }
}
