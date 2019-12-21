using Sleeper.Core.Enums;

namespace Sleeper.Core.Interfaces
{
    public interface ILogger
    {
        void LogMessage(LogLevel level, string message);
    }
}
