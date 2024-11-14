using NUnit.Framework;
using Server.Observer;
using System.Collections.Generic;
using Server;
using System.Reflection;

namespace Tests
{
    public class StubObserver : IObserver
    {
        public bool UpdateCalled { get; private set; }
        public GameData? ReceivedGameData { get; private set; }

        public void Update(GameData gameData)
        {
            UpdateCalled = true;
            ReceivedGameData = gameData;
        }

        public void Reset()
        {
            UpdateCalled = false;
            ReceivedGameData = null;
        }
    }

    [TestFixture]
    public class ObserversTests
    {
        private Game _subject;
        private StubObserver _observer1;
        private StubObserver _observer2;

        [SetUp]
        public void Setup()
        {
            // Initialize Game as the subject
            _subject = new Game();
            _observer1 = new StubObserver();
            _observer2 = new StubObserver();
        }

        [Test]
        public void AttachObserver_AddsObserverToSubject()
        {
            // Attach observer to subject
            _subject.Attach(_observer1);

            // Notify observers
            _subject.Notify();

            // Assert that observer received the notification
            Assert.IsTrue(_observer1.UpdateCalled, "Observer1 should receive update after Attach and Notify.");
        }

        [Test]
        public void DetachObserver_RemovesObserverFromSubject()
        {
            // Attach and then detach observer
            _subject.Attach(_observer1);
            _subject.Detach(_observer1);

            // Notify observers
            _subject.Notify();

            // Assert that observer did not receive the notification
            Assert.IsFalse(_observer1.UpdateCalled, "Observer1 should not receive update after Detach.");
        }

        [Test]
        public void NotifyObservers_SendsGameDataToAllAttachedObservers()
        {
            // Attach multiple observers
            _subject.Attach(_observer1);
            _subject.Attach(_observer2);

            // Create and set sample GameData directly
            var gameData = new GameData
            {
                CurrentPlayer = "Player1",
                StatusMessage = "Test Fire Notification",
                RecentMoves = new List<Move> { new Move { PlayerSeat = 1, X = 5, Y = 3, Hit = true } }
            };

            // Use reflection to set `gameData` in Game directly
            typeof(Game).GetField("gameData", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(_subject, gameData);

            // Call Notify directly to simulate observers receiving GameData
            _subject.Notify();

            // Assert that each observer received the update
            Assert.IsTrue(_observer1.UpdateCalled, "Observer1 should receive update after Notify.");
            Assert.IsTrue(_observer2.UpdateCalled, "Observer2 should receive update after Notify.");

            // Verify the GameData in each observer
            Assert.AreEqual("Player1", _observer1.ReceivedGameData.CurrentPlayer);
            Assert.AreEqual("Test Fire Notification", _observer1.ReceivedGameData.StatusMessage);
            Assert.AreEqual(1, _observer1.ReceivedGameData.RecentMoves.Count);
            Assert.AreEqual(5, _observer1.ReceivedGameData.RecentMoves[0].X);
            Assert.AreEqual(3, _observer1.ReceivedGameData.RecentMoves[0].Y);
            Assert.AreEqual(true, _observer1.ReceivedGameData.RecentMoves[0].Hit);

            // Repeat for Observer2
            Assert.AreEqual("Player1", _observer2.ReceivedGameData.CurrentPlayer);
            Assert.AreEqual("Test Fire Notification", _observer2.ReceivedGameData.StatusMessage);
        }
    }
}
