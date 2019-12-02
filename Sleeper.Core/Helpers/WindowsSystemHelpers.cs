namespace Sleeper.Core.Helpers
{
    public static class WindowsSystemHelpers
    {
        public static void PerformSleep()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo()
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = "rundll32.exe",
                Arguments = "powrprof.dll,SetSuspendState 0,1,0",
            };
            process.StartInfo = startInfo;
            process.Start();
        }
    }
}
