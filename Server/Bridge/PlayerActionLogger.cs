using System;
using Server.Adapter;

namespace Server.Bridge
{
    public class PlayerActionLogger : Logger
    {
        public PlayerActionLogger(ILogger implementation) : base(implementation)
        {
        }

        public override void Log(string eventType, string message)
        {
            Implementation.LogEvent(eventType, $"[PlayerAction] {message}", DateTime.UtcNow);
        }
    }
}