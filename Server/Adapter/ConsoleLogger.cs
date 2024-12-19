using System;

namespace Server.Adapter
{
    public class ConsoleLogger : ILogger
    {
        public void LogEvent(string eventType, string message, DateTime timestamp)
        {
            Console.WriteLine($"{timestamp}: {eventType} - {message}");
        }
    }
}