using System.Diagnostics;

namespace Sleeper.Core.Helpers
{
    public static class WindowsSystemHelpers
    {
        public static void PerformSleep()
        {
            RunSystemCommand("rundll32.exe", "powrprof.dll,SetSuspendState 0,1,0");
        }

        public static void RunSystemCommand(string fileName, string arguments)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = fileName,
                Arguments = arguments,
            };
            process.StartInfo = startInfo;
            process.Start();
        }

        public static void SetHibernate(bool hibernateEnabled)
        {
            var arguments = $"/hibernate {(hibernateEnabled ? "on" : "off")}";
            RunSystemCommand("powercfg.exe", arguments);
        }
    }
}
