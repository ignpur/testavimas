using System;
using System.IO;

namespace Server.Adapter
{
    public class FileLogger : ILogger
    {
        private readonly string _filePath;

        public FileLogger(string filePath)
        {
            _filePath = filePath;
            
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public void LogEvent(string eventType, string message, DateTime timestamp)
        {
            string logEntry = $"{timestamp}: {eventType} - {message}{Environment.NewLine}";
            File.AppendAllText(_filePath, logEntry);
        }
    }
}