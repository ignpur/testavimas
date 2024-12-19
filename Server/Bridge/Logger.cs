using Server.Adapter;

namespace Server.Bridge
{
    public abstract class Logger
    {
        protected readonly ILogger Implementation;
        
        protected Logger(ILogger implementation)
        {
            Implementation = implementation;
        }
        
        public abstract void Log(string eventType, string message);
    }
}