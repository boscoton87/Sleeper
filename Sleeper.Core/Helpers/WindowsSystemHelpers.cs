using Facade.Services;
using Sleeper.Core.Interfaces;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Sleeper.Core.Helpers
{
    public static class WindowsSystemHelpers
    {
        public static void PerformSleep()
        {
            var settings = Container.ResolveGlobalInstance<ISettingLoader>().GetSettings();
            var modernStandbyEnabled = bool.Parse(settings["modernStandbyEnabled"]);
            var hibernateEnabled = bool.Parse(settings["hibernateEnabled"]);
            if (modernStandbyEnabled && !hibernateEnabled)
            {
                SendMessage(0xFFFF, 0x112, 0xF170, 2);
            } else
            {
                RunSystemCommand("rundll32.exe", "powrprof.dll,SetSuspendState 0,1,0");
            }
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
            process.WaitForExit();
        }

        public static void SetHibernate(bool hibernateEnabled)
        {
            var arguments = $"/hibernate {(hibernateEnabled ? "on" : "off")}";
            RunSystemCommand("powercfg.exe", arguments);
        }

        [DllImport("user32.dll")]
        private static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);
    }
}
