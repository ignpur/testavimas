using System;
using Server.Adapter;

namespace Server.Bridge
{
    public class GameEventLogger : Logger
    {
        public GameEventLogger(ILogger implementation) : base(implementation)
        {
        }

        public override void Log(string eventType, string message)
        {
            Implementation.LogEvent(eventType, $"[GameEvent] {message}", DateTime.UtcNow);
        }
    }
}