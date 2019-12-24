using Facade.Services;
using Sleeper.Core.Enums;
using Sleeper.Core.Interfaces;
using System.Collections.Generic;

namespace Sleeper.Core.Helpers
{
    public static class ReportingHelpers
    {
        public static void LogInfo(string message, bool includeSettings = true)
        {
            Container.ResolveGlobalInstance<ILogger>().LogMessage(LogLevel.Info, message, includeSettings ? GetSettings() : null);
        }

        public static void LogWarning(string message, bool includeSettings = true)
        {
            Container.ResolveGlobalInstance<ILogger>().LogMessage(LogLevel.Warning, message, includeSettings ? GetSettings() : null);
        }

        public static void LogError(string message, bool includeSettings = true)
        {
            Container.ResolveGlobalInstance<ILogger>().LogMessage(LogLevel.Error, message, includeSettings ? GetSettings() : null);
        }

        private static Dictionary<string, string> GetSettings()
        {
            return Container.ResolveGlobalInstance<ISettingManager>().GetSettings();
        }
    }
}
