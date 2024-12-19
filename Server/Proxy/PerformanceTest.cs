using System;
using System.Diagnostics;
using Server.Command;
using System.Drawing;
using Server.Strategy.ShipPlacement;

namespace Server.Proxy
{
    public class PerformanceTest
    {
        public static void TestGameInitialization()
        {
            long before = GC.GetTotalMemory(true);

            for (int i = 0; i < 1000000; i++) // Repeat 1000 times
            {
                IGame game = new Game();
                game.setBoardChoice();
                bool player1Joined = game.JoinPlayer($"Player{i}_1", 0, new RandomShipPlacementStrategy());
                bool player2Joined = game.JoinPlayer($"Player{i}_2", 1, new RandomShipPlacementStrategy());

            }
            long after = GC.GetTotalMemory(true);
            Console.WriteLine($"Memory allocated during initialization: {(after - before) / 1024} MB");

        }
        public static void TestProxyGameInitialization()
        {
            long before = GC.GetTotalMemory(true);
            for (int i = 0; i < 1000000; i++) // Repeat 1000 times
            {
                IGame game = new GameProxy();
                game.setBoardChoice();
                bool player1Joined = game.JoinPlayer($"Player{i}_1", 0, new RandomShipPlacementStrategy());
                bool player2Joined = game.JoinPlayer($"Player{i}_2", 1, new RandomShipPlacementStrategy());

            }
            long after = GC.GetTotalMemory(true);
            Console.WriteLine($"Memory allocated during initialization: {(after - before) / 1024} MB");
        }

        public static void TestShipPlacement(IGame game)
        {
            long before = GC.GetTotalMemory(true);

            game.setBoardChoice();
            game.JoinPlayer("Player1", 0, new MixedShipPlacementStrategy());
            game.JoinPlayer("Player2", 1, new RandomShipPlacementStrategy());
            game.State = GameState.ArrangingShips;

            for (int i = 0; i < 1000000; i++) // Place 10,000 ships
            {
                int x = i % 10; // Randomize coordinates
                int y = (i / 10) % 10;
                var result = game.SetShip(0, 3, x, y, false);

                //Console.WriteLine("Ship placed: {0}", command.GetResults().result);
            }

            long after = GC.GetTotalMemory(true);
            Console.WriteLine($"Memory allocated during ship placement: {(after - before) / 1024} MB");
        }
        public static void TestLargeNumberOfShots(IGame game)
        {
            long before = GC.GetTotalMemory(true);

            game.setBoardChoice();
            game.JoinPlayer("Player1", 0, new RandomShipPlacementStrategy());
            game.JoinPlayer("Player2", 1, new RandomShipPlacementStrategy());
            game.Start();

            for (int i = 0; i < 1000000; i++) // Fire 1,000,000 shots
            {
                int x = i % 10;
                int y = (i / 10) % 10;

                game.Turn = 0;
                var shot = game.Fire(0, x, y);
            }

            long after = GC.GetTotalMemory(true);
            Console.WriteLine($"Memory allocated during shooting: {(after - before) / 1024} MB");
        }
        public static void TestUndoRedo(IGame game)
        {
            long before = GC.GetTotalMemory(true);

            game.setBoardChoice();
            game.JoinPlayer("Player1", 0, new MixedShipPlacementStrategy());
            game.JoinPlayer("Player2", 1, new RandomShipPlacementStrategy());
            game.State = GameState.ArrangingShips;

            for (int i = 0; i < 1000000; i++) // Place 10,000 ships
            {
                int x = i % 10; // Randomize coordinates
                int y = (i / 10) % 10;
                var result = game.SetShip(0, 3, x, y, false);

                //Console.WriteLine("Ship placed: {0}", command.GetResults().result);

                game.Undo(0);
            }

            long after = GC.GetTotalMemory(true);
            Console.WriteLine($"Memory allocated during Undo: {(after - before)/1024} MB");
        }


        //-------------------------------------------------------------------------//
        private static void TestGame(IGame game)
        {
            game.JoinPlayer("Player1", 0, new RandomShipPlacementStrategy());
            game.JoinPlayer("Player2", 1, new RandomShipPlacementStrategy());
            game.Start();

            for (int i = 0; i < 1000; i++) // Simulate many operations
            {
                game.SetShip(0, 3, i % 10, i % 10, false);
                game.Fire(0, i % 10, i % 10);
            }
        }

        public static void RunTest()
        {
            IGame game = new Game();
            IGame proxyGame = new GameProxy();

            Stopwatch stopwatch = new Stopwatch();

            Console.WriteLine("Testing Direct Game:");
            stopwatch.Start();
            TestGame(game);
            stopwatch.Stop();
            Console.WriteLine($"Execution Time: {stopwatch.ElapsedMilliseconds} ms");

            Console.WriteLine("\nTesting GameProxy:");
            stopwatch.Restart();
            TestGame(proxyGame);
            stopwatch.Stop();
            Console.WriteLine($"Execution Time: {stopwatch.ElapsedMilliseconds} ms");
        }

        private static long MeasureMemoryUsage(Action action)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            long before = GC.GetTotalMemory(true);
            action.Invoke();
            long after = GC.GetTotalMemory(true);
            return after - before;
        }

        public static void RunMemoryTest()
        {
            IGame game = new Game();
            IGame proxyGame = new GameProxy();

            Console.WriteLine("Measuring Memory Usage:");
            long directMemory = MeasureMemoryUsage(() => TestGame(game));
            Console.WriteLine($"Direct Game Memory Usage: {directMemory} bytes");

            long proxyMemory = MeasureMemoryUsage(() => TestGame(proxyGame));
            Console.WriteLine($"GameProxy Memory Usage: {proxyMemory} bytes");
        }
    }
}
