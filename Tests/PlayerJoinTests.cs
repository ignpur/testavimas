using Server.AbstractFactory;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Strategy.ShipPlacement;


namespace Tests
{
    public class PlayerJoinTests
    {
        private Game _game;

        [SetUp]
        public void Setup()
        {
            _game = new Game();
        }

        [Test]
        public void JoinPlayer_WithSeatZero_ReturnsPlayerOne()
        {
            var result = _game.JoinPlayer("Player1", 0, null);

            Assert.IsTrue(result, "Expected JoinPlayer to return true for seat 0.");
            Assert.IsInstanceOf<PlayerOne>(_game.Players[0], "Expected Players[0] to be of type PlayerOne.");
        }

        [Test]
        public void JoinPlayer_WithSeatOne_ReturnsPlayerTwo()
        {
            var result = _game.JoinPlayer("Player2", 1, null);

            Assert.IsTrue(result, "Expected JoinPlayer to return true for seat 1.");
            Assert.IsInstanceOf<PlayerTwo>(_game.Players[1], "Expected Players[1] to be of type PlayerTwo.");
        }

        [Test]
        public void JoinPlayer_WithInvalidSeat_ReturnsFalse()
        {
            var result = _game.JoinPlayer("Player3", 2, null);

            Assert.IsFalse(result, "Expected JoinPlayer to return false for invalid seat value.");
            Assert.IsNull(_game.Players[0], "Expected Players[0] to be null for invalid seat.");
            Assert.IsNull(_game.Players[1], "Expected Players[1] to be null for invalid seat.");
        }
    }
}
