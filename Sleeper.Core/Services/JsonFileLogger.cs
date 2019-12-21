using Newtonsoft.Json;
using Sleeper.Core.Enums;
using Sleeper.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sleeper.Core.Services
{
    public class JsonFileLogger : ILogger
    {
        private string LogPath { get; }
        private LogLevel MinimumLevel { get; }
        public JsonFileLogger(string logPath, LogLevel minimumLevel)
        {
            LogPath = logPath;
            MinimumLevel = minimumLevel;
        }
        void ILogger.LogMessage(LogLevel level, string message)
        {
            if (level >= MinimumLevel && !string.IsNullOrWhiteSpace(message))
            {
                List<LogEntry> logEntries = new List<LogEntry>();
                if (File.Exists(LogPath))
                {
                    var contents = File.ReadAllText(LogPath);
                    try
                    {
                        logEntries = JsonConvert.DeserializeObject<List<LogEntry>>(contents);
                    }
                    catch
                    {
                        logEntries.Add(new LogEntry(LogLevel.Warning, "Previous log entries corrupt, rebuilding log file", DateTime.UtcNow));
                    }
                }
                logEntries.Add(new LogEntry(level, message, DateTime.UtcNow));
                File.WriteAllText(LogPath, JsonConvert.SerializeObject(logEntries));
            }
        }

        private class LogEntry
        {
            public LogEntry(LogLevel level, string message, DateTime timeStamp)
            {
                Level = level;
                Message = message;
                TimeStamp = timeStamp;
            }
            public LogLevel Level { get; }

            public string Message { get; }

            public DateTime TimeStamp { get; }
        }
    }
}
