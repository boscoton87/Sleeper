using Sleeper.Core.Enums;
using System.Collections.Generic;

namespace Sleeper.Core.Interfaces
{
    public interface ILogger
    {
        void LogMessage(LogLevel level, string message, Dictionary<string, string> settings);
    }
}
