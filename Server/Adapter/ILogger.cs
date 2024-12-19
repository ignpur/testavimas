using System;

namespace Server.Adapter
{
    public interface ILogger
    {
        void LogEvent(string eventType, string message, DateTime timestamp);
    }
}