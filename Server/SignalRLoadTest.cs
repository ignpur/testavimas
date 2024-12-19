using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Server
{
    /*public class SignalRLoadTest
    {
        private static int connectedUsers = 0;
        private static int successfulActions = 0;
        private static int failedConnections = 0;

        public static async Task SimulateUser(string hubUrl, string gameId, string userName, int seat, string strategy)
        {
            try
            {
                // Establish connection to SignalR Hub
                var connection = new HubConnectionBuilder()
                    .WithUrl(hubUrl)
                    .WithAutomaticReconnect()
                    .Build();

                connection.On<string>("GameCreated", (id) =>
                {
                    Console.WriteLine($"Game Created: {id}");
                });

                await connection.StartAsync();
                Interlocked.Increment(ref connectedUsers);

                // Join the game
                await connection.InvokeAsync("Join", gameId, userName, seat, strategy);

                // Simulate placing a ship
                await connection.InvokeAsync("SetShip", gameId, seat, 3, 0, 0, false);

                // Simulate firing a shot
                await connection.InvokeAsync("Fire", gameId, seat, 1, 1);

                // Simulate readying up
                await connection.InvokeAsync("ReadyUp", gameId, seat);

                Interlocked.Increment(ref successfulActions);

                await connection.StopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect or perform actions: {ex.Message}");
                Interlocked.Increment(ref failedConnections);
            }
        }

        public static async Task RunLoadTest(string hubUrl, string gameId, int userCount, int durationSeconds)
        {
            Console.WriteLine($"Starting SignalR Load Test with {userCount} users...");

            Stopwatch stopwatch = Stopwatch.StartNew();
            Task[] tasks = new Task[userCount];

            for (int i = 0; i < userCount; i++)
            {
                string userName = $"User_{i}";
                tasks[i] = SimulateUser(hubUrl, gameId, userName, i % 2, "Random Placement");
            }

            await Task.WhenAll(tasks);
            stopwatch.Stop();

            Console.WriteLine("Load Test Completed.");
            Console.WriteLine($"Connected Users: {connectedUsers}");
            Console.WriteLine($"Successful Actions: {successfulActions}");
            Console.WriteLine($"Failed Connections: {failedConnections}");
            Console.WriteLine($"Total Duration: {stopwatch.Elapsed.TotalSeconds} seconds");
        }

        public static async Task Main(string[] args)
        {
            string hubUrl = "http://localhost:4000/hub"; // Adjust your hub URL
            string gameId = Guid.NewGuid().ToString();   // Create a unique game ID
            int userCount = 1000;
            int durationSeconds = 300;

            await RunLoadTest(hubUrl, gameId, userCount, durationSeconds);
        }
    }*/
}
