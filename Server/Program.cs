using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Proxy;
using System.Runtime.CompilerServices;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //PerformanceTest.RunTest();
            //Console.WriteLine();
            //PerformanceTest.RunMemoryTest();
            //Console.WriteLine();
            //IGame proxyGame = new GameProxy();

            PerformanceTest.TestGameInitialization();
            //PerformanceTest.TestProxyGameInitialization();
            PerformanceTest.TestShipPlacement(new Game());
            PerformanceTest.TestLargeNumberOfShots(new Game());
            PerformanceTest.TestUndoRedo(new Game());
            Console.WriteLine("Test completed");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseUrls("http://*:4000")
                    .UseStartup<Startup>();
                });
    }
}
