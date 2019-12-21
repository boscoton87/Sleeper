using Facade.Services;
using Sleeper.Core.Enums;
using Sleeper.Core.Interfaces;

namespace Sleeper.Core.Helpers
{
    public static class ReportingHelpers
    {
        public static void LogInfo(string message)
        {
            Container.ResolveGlobalInstance<ILogger>().LogMessage(LogLevel.Info, message);
        }

        public static void LogWarning(string message)
        {
            Container.ResolveGlobalInstance<ILogger>().LogMessage(LogLevel.Warning, message);
        }

        public static void LogError(string message)
        {
            Container.ResolveGlobalInstance<ILogger>().LogMessage(LogLevel.Error, message);
        }
    }
}
